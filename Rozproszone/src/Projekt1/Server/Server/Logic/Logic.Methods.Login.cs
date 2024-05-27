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

        public sealed class OnClient : Client.ITokenProvider
        {
            public async Task<Sockets.Token> Handle
            (
                Sockets.Handler socket
            )
            {
                while(true)
                {
                    var login = Client.ReadValue<string>("Login");

                    var password = Client.ReadValue<string>("Password");

                    var (success, message, token) = await Handle(socket, login!, password!);

                    if(success == false)
                    {
                        Client.Value($"Login Failed: {message}");
                        continue;
                    }

                    Client.Value($"Login Successful");
                    if(token.IsAdmin)
                    {
                        Client.Value("[--ADMIN--]");
                    }
                    return token;
                }
            }

            private static async Task<Response> Handle
            (
                Sockets.Handler socket,
                string login,
                string password
            )
            {
                Request request = new (login!, password!);

                await socket.SendMessage(request);

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
                var (login, password) = message;

                var (status, resultMessage) = Account.LoggedIn.TryLogin(_dataBase, login, password, out var loggedIn);

                if(status == true)
                {
                    var token = loggedIn.LoginToken;

                    var resp = new Response(status, resultMessage, token);

                    await socket.SendMessage(resp);

                    return;
                }

                var response = new Response(status, resultMessage, null!);

                await socket.SendMessage(response);
            }
        }
    }
}