using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Info
    {
        public sealed record Request(Sockets.Token Token) : Sockets.ITokenMessage
        {
            public static string Command => "Info";
        }

        private sealed record Response(AccountDTO Account) : Sockets.IMessage
        {
            public static string Command => "InfoResp";
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
                    .AppendLine($"Name: {name}")
                    .AppendLine($"Surname: {surname}")
                    .AppendLine($"Balance: {balance}");
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

                var response = new Response(new (loggedIn.Account));

                await socket.SendMessage(response);
            }
        }
    }
}