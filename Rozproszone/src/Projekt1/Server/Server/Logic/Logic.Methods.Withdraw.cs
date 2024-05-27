using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Withdraw
    {
        public sealed record Request(Sockets.Token Token, int Amount) : Sockets.ITokenMessage
        {
            public static string Command => "Withdraw";
        }

        private sealed record Response(bool Success, int Balance) : Sockets.IMessage
        {
            public static string Command => "WithdrawResp";
        }

        public sealed class OnClient : Client.IHandle
        {
            public string ConsoleCommand => "Withdraw";

            public async Task<StringBuilder> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                Client.Input();

                var amount = Client.ReadValue<int>("Amount:");

                var (success, balance) = await Handle(socket, token,  amount);

                return new StringBuilder()
                    .Append(success ? "Success" : $"Failed")
                    .Append($"Balance: {balance}");
            }

            private static async Task<Response> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token,
                int amount
            )
            {
                var message = new Request(token, amount);

                await socket.SendMessage(message);

                return await socket.TryRecieveMessage<Response>();
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

                var result = loggedIn.TryWithdraw(message.Amount);

                var response = new Response(result, loggedIn.Account.Balance);

                await socket.SendMessage(response);
            }
        }
    }
}