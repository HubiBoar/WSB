using System.Net.Sockets;
using System.Text.Json;
using Shared;

namespace Server.Public;

public static partial class Logic
{

    public static class EditInfo
    {
        private sealed record Request(string Name, string Surname);
        private sealed record Response(Account Account);

        private const string Send = "EditInfo";
        private const string Resp = "EditInfo";

        public static async Task<Account> OnClient
        (
            Socket socket,
            Token token,
            string name,
            string surname
        )
        {
            var message = PayloadWithToken<Request>.ToMessage(Send, token, new Request(name, surname));

            await socket.SendMessage(message);

            while(true)
            {
                var response = await socket.RecieveMessage();

                if(response.Command != Resp)
                {
                    continue;
                }

                var result = response.GetPayload<Response>().Payload;

                return result.Account;
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

            loggedIn.Account.EditUserData(payload.Payload.Name, payload.Payload.Surname);
            var response = PayloadWithToken<Response>.ToMessage(Resp, new Token(token.Value), new Response(new (loggedIn.Account)))!;

            await socket.SendMessage(response);

            return true;
        }
    }
}