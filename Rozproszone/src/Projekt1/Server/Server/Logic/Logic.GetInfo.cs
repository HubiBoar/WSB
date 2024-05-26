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

        private sealed record Response(Account Account) : Sockets.IMessage
        {
            public static string Command => "GetInfoResp";
        }

        public static async Task<Account> OnClient
        (
            Sockets.Handler socket,
            Sockets.Token token
        )
        {
            var message = new Request(token);

            await socket.SendMessage(message);

            while(true)
            {
                var (success, response) = await socket.TryRecieveMessage<Response>();

                if(success == false)
                {
                    continue;
                }

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