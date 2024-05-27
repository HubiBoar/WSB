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
                Client.Input();

                var amount = Client.ReadValue<int>("Amount:");

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

        internal static async Task OnServer
        (
            Sockets.Handler socket,
            Request message,
            Server.Account.DataBase dataBase
        )
        {
            Server.Account.LoggedIn.TryLogin(dataBase, message.Token, out var loggedIn);

            loggedIn.Account.Deposit(message.Amount);

            var response = new Response(loggedIn.Account.Balance);

            await socket.SendMessage(response);
        }
    }
}