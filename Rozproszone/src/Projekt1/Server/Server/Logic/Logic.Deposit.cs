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

        public static async Task<int> OnClient
        (
            Sockets.Handler socket,
            Sockets.Token token,
            int amount
        )
        {
            var message = new Request(token, amount);

            await socket.SendMessage(message);

            while(true)
            {
                var (success, response) = await socket.TryRecieveMessage<Response>();

                if(success == false)
                {
                    continue;
                }

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