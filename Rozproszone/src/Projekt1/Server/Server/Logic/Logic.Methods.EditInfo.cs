using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{

    public static class EditInfo
    {
        public sealed record Request(Sockets.Token Token, string Name, string Surname) : Sockets.ITokenMessage
        {
            public static string Command => "EditInfo";
        }

        private sealed record Response(AccountDTO Account) : Sockets.IMessage
        {
            public static string Command => "EditInfoResp";
        }

        public sealed class OnClient : Client.IHandle
        {
            public string ConsoleCommand => "EditInfo";

            public async Task<StringBuilder> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                Client.Input();

                var name = Client.ReadValue<string>("Name:")!;
                var surname = Client.ReadValue<string>("Surname:")!;

                var (newName, newSurname, balance, _) = await Handle(socket, token, name, surname);

                return new StringBuilder()
                    .Append($"Name: {newName}")
                    .Append($"Surname: {newSurname}")
                    .Append($"Balance: {balance}");
            }

            private static async Task<AccountDTO> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token,
                string name,
                string surname
            )
            {
                var message = new Request(token, name, surname);

                await socket.SendMessage(message);

                var response = await socket.TryRecieveMessage<Response>();

                return response.Account;
            }
        }

        internal static async Task OnServer
        (
            Sockets.Handler socket,
            Request message,
            Server.Account.DataBase dataBase
        )
        {
            Server.Account.LoggedIn.TryLogin(dataBase, message.Token, out var loggedIn);

            loggedIn.Account.EditUserData(message.Name, message.Surname);

            var response = new Response(new (loggedIn.Account));

            await socket.SendMessage(response);
        }
    }
}