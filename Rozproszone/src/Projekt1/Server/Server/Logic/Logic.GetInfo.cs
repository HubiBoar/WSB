namespace Server.Public;

public static partial class Logic
{
    public static class GetInfo
    {
        private sealed record Response(Account Account);

        private const string Send = "GetInfo";
        private const string Resp = "GetInfoResp";

        public static async Task<Account> OnClient
        (
            Socket socket,
            Token token
        )
        {
            var message = PayloadWithToken.ToMessage(Send, token);

            await socket.SendMessage(message);

            while(true)
            {
                var response = await socket.RecieveMessage();

                if(response.Command != Resp)
                {
                    continue;
                }

                var account = response.GetPayload<Response>().Payload.Account;

                return account;
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

            var tokenFromPayload = message.GetToken();

            var token = new Server.Account.Token(tokenFromPayload.Value);

            Server.Account.LoggedIn.TryLogin(dataBase, token, out var loggedIn);

            var response = PayloadWithToken<Response>.ToMessage(Resp, new Token(token.Value), new Response(new Account(loggedIn.Account)))!;

            await socket.SendMessage(response);

            return true;
        }
    }
}