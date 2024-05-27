using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Deposit
    {
        public sealed record Request(Sockets.Token Token, int Amount) : Sockets.ITokenMessage
        {
            public static string Command => "Deposit";
        }

        private sealed record Response(int Balance) : Sockets.IMessage
        {
            public static string Command => "DepositResp";
        }

        public sealed class OnClient : Client.IHandle
        {
            public string ConsoleCommand => "Deposit";

            public async Task<StringBuilder> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                Print.Input();

                var amount = Print.ReadValue<int>("Amount:");

                var balance = await Handle(socket, token, amount);

                return new StringBuilder($"Balance: {balance}");
            }

            private static async Task<int> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token,
                int amount
            )
            {
                var message = new Request(token, amount);

                await socket.SendMessage(message);

                var response = await socket.TryRecieveMessage<Response>();

                return response.Balance;
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

                loggedIn.Account.Deposit(message.Amount);

                var response = new Response(loggedIn.Account.Balance);

                await socket.SendMessage(response);
            }
        }
    }
}