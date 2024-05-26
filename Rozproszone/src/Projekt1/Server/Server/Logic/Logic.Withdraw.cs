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

        public static async Task<(bool Success, int Balance)> OnClient
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

                return (response.Success, response.Balance);
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

            var result = loggedIn.TryWithdraw(message.Amount);

            var response = new Response(result, loggedIn.Account.Balance);

            await socket.SendMessage(response);
        }
    }
}