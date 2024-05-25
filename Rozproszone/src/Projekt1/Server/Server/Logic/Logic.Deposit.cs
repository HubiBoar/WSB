namespace Server.Public;

public static partial class Logic
{
    public static class Deposit
    {
        private sealed record Request(int Amount);
        private sealed record Response(int Balance);

        private const string Send = "Deposit";
        private const string Resp = "DepositResp";

        public static async Task<int> OnClient
        (
            Socket socket,
            Token token,
            int amount
        )
        {
            var message = PayloadWithToken<Request>.ToMessage(Send, token, new Request(amount));

            await socket.SendMessage(message);

            while(true)
            {
                var response = await socket.RecieveMessage();

                if(response.Command != Resp)
                {
                    continue;
                }

                return response.GetPayload<Response>().Payload.Balance;
            }
        }

        internal static async Task<bool> OnServer
        (
            Socket socket,
            Sockets.Message message,
            Server.Account.DataBase dataBase
        )
        {
            if(message.Command != Send)
            {
                return false;
            }

            var payload = message.GetPayload<Request>();

            var token = new Server.Account.Token(payload.Token.Value);

            Server.Account.LoggedIn.TryLogin(dataBase, token, out var loggedIn);

            loggedIn.Account.Deposit(payload.Payload.Amount);

            var response = PayloadWithToken<Response>.ToMessage(Resp, new Token(token.Value), new Response(loggedIn.Account.Balance))!;

            await socket.SendMessage(response);

            return true;
        }
    }
}