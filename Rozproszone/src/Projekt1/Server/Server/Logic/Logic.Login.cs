using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Login
    {
        public sealed record Request(string Login, string Password) : Sockets.IMessage
        {
            public static string Command => "LoginToken";
        }

        private sealed record Response(bool Success, string Message, Sockets.Token Token) : Sockets.IMessage
        {
            public static string Command => "LoginTokenResp";
        }

        public static async Task<Sockets.Token> OnClient
        (
            Sockets.Handler socket
        )
        {
            while(true)
            {
                Console.WriteLine("Login");
                var login = Console.ReadLine();
                Console.WriteLine("Password");
                var password = Console.ReadLine();

                Request request = new (login!, password!);

                await socket.SendMessage(request);

                var response = await socket.TryRecieveMessage<Response>();

                Console.WriteLine($"Login Result: {response.Message}");

                if(response.Success == false)
                {
                    continue;
                }

                return response.Token;
            }
        }

        internal static async Task OnServer
        (
            Sockets.Handler socket,
            Request message,
            Server.Account.DataBase dataBase
        )
        {
            Console.WriteLine("OnServer");

            var (login, password) = message;

            var (status, resultMessage) = Server.Account.LoggedIn.TryLogin(dataBase, login, password, out var loggedIn);

            if(status == true)
            {
                var token = loggedIn.LoginToken;

                var resp = new Response(status, resultMessage, token);

                Console.WriteLine(resp.Message);
                await socket.SendMessage(resp);

                return;
            }

            var response = new Response(status, resultMessage, null!);

            await socket.SendMessage(response);
        }
    }
}