using System.Net.Sockets;
using System.Text.Json;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Login
    {
        private sealed record Response(bool Success, string Message);

        private const string Send = "LoginToken";
        private const string Resp = "LoginTokenResp";

        public static async Task<Token> OnClient
        (
            Socket socket
        )
        {
            while(true)
            {
                Console.WriteLine("Login");
                var login = Console.ReadLine();
                Console.WriteLine("Password");
                var password = Console.ReadLine();

                Server.Account.Token token = new (login!, password!);

                var message = PayloadWithToken.ToMessage(Send, new Token(token.Value));

                await socket.SendMessage(message);

                while(true)
                {
                    var response = await socket.RecieveMessage();

                    if(response.Command != Resp)
                    {
                        continue;
                    }

                    var (success, responseMessage) = response.GetPayload<Response>().Payload;

                    Console.WriteLine($"Login Result: {responseMessage}");

                    if(success)
                    {
                        return new (token.Value);
                    }

                    break;
                }
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

            var token = message.GetToken();

            var result = Server.Account.LoggedIn.TryLogin(dataBase, new (token.Value), out var _);

            var response = PayloadWithToken<Response>.ToMessage(Resp, token, new Response(result.status, result.message))!;

            await socket.SendMessage(response);

            return true;
        }
    }
}