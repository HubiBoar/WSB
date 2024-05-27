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
                Print.Input();

                var name = Print.ReadValue<string>("Name:")!;
                var surname = Print.ReadValue<string>("Surname:")!;

                var (newName, newSurname, balance, _) = await Handle(socket, token, name, surname);

                return new StringBuilder()
                    .AppendLine($"Name: {newName}")
                    .AppendLine($"Surname: {newSurname}")
                    .AppendLine($"Balance: {balance}");
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

        internal sealed class OnServer : Server.IHandle<Request>
        {
            private readonly Account.DataBase _dataBase;

            public OnServer(Account.DataBase dataBase)
            {
                _dataBase = dataBase;
            }

            public async Task Handle(Sockets.Handler socket, Request message)
            {
                Account.LoggedIn.TryLogin(_dataBase, message.Token, out var loggedIn);

                loggedIn.Account.EditUserData(message.Name, message.Surname);

                var response = new Response(new (loggedIn.Account));

                await socket.SendMessage(response);
            }
        }
    }
}