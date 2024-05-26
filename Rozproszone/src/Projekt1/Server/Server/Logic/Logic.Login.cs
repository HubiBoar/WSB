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

                while(true)
                {
                    var (success, response) = await socket.TryRecieveMessage<Response>();

                    if(success == false)
                    {
                        continue;
                    }

                    Console.WriteLine($"Login Result: {response}");

                    if(success)
                    {
                        return response.Token;
                    }

                    break;
                }
            }
        }

        internal static async Task OnServer
        (
            Sockets.Handler socket,
            Request message,
            Server.Account.DataBase dataBase
        )
        {
            var (login, password) = message;

            var result = Server.Account.LoggedIn.TryLogin(dataBase, login, password, out var loggedIn);

            var token = loggedIn.LoginToken;

            var response = new Response(result.status, result.message, token);

            await socket.SendMessage(response);
        }
    }
}