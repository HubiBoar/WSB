using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class GetInfo
    {
        public sealed record Request(Sockets.Token Token) : Sockets.ITokenMessage
        {
            public static string Command => "GetInfo";
        }

        private sealed record Response(AccountDTO Account) : Sockets.IMessage
        {
            public static string Command => "GetInfoResp";
        }

        public sealed class OnClient : Client.IHandle
        {
            public string ConsoleCommand => "Info";

            public async Task<StringBuilder> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                var (name, surname, balance, _) = await HandleLocal(socket, token);

                return new StringBuilder()
                    .Append($"Name: {name}")
                    .Append($"Surname: {surname}")
                    .Append($"Balance: {balance}");
            }

            private static async Task<AccountDTO> HandleLocal
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                var message = new Request(token);

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

            var response = new Response(new (loggedIn.Account));

            await socket.SendMessage(response);
        }
    }
}