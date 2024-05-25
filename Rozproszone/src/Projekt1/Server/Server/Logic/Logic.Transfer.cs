using System.Net.Sockets;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Transfer
    {
        private sealed record Request(string ToLogin, int Amount);
        private sealed record Response(bool Success, string Message, int Balance);

        private const string Send = "Transfer";
        private const string Resp = "TransferResp";

        public static async Task<(bool Success, int Balance)> OnClient
        (
            Socket socket,
            Token token,
            string toLogin,
            int amount
        )
        {
            var message = PayloadWithToken<Request>.ToMessage(Send, token, new Request(toLogin, amount));

            await socket.SendMessage(message);

            while(true)
            {
                var response = await socket.RecieveMessage();

                if(response.Command != Resp)
                {
                    continue;
                }

                var result = response.GetPayload<Response>().Payload;

                return (result.Success, result.Balance);
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

            if(dataBase.Accounts.TryGetValue(payload.Payload.ToLogin, out var target) == false)
            {
                var resp = PayloadWithToken<Response>.ToMessage(Resp, new Token(token.Value), new Response(false, "Target not found", loggedIn.Account.Balance))!;

                await socket.SendMessage(resp);

                return true;
            }

            var result = loggedIn.TryTransfer(target, payload.Payload.Amount);

            var response = PayloadWithToken<Response>.ToMessage(Resp, new Token(token.Value), new Response(result, "Not enought founds", loggedIn.Account.Balance))!;

            await socket.SendMessage(response);

            return true;
        }
    }
}