using System.Net.Sockets;
using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static class Transfer
    {
        public sealed record Request(Sockets.Token Token, string ToLogin, int Amount) : Sockets.ITokenMessage
        {
            public static string Command => "Transfer";
        }

        private sealed record Response(bool Success, string Message, int Balance) : Sockets.IMessage
        {
            public static string Command => "TransferResp";
        }

        public sealed class OnClient : Client.IHandle
        {
            public string ConsoleCommand => "Transfer";

            public async Task<StringBuilder> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token
            )
            {
                Client.Input();

                var to = Client.ReadValue<string>("To:");
                var amount = Client.ReadValue<int>("Amount:");

                var (success, message, balance) = await Handle(socket, token, to, amount);

                return new StringBuilder()
                    .Append(success ? "Success" : $"Failed: {message}")
                    .Append($"Balance: {balance}");
            }

            private static async Task<Response> Handle
            (
                Sockets.Handler socket,
                Sockets.Token token,
                string toLogin,
                int amount
            )
            {
                var message = new Request(token, toLogin, amount);

                await socket.SendMessage(message);

                return await socket.TryRecieveMessage<Response>();
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

            if(dataBase.Accounts.TryGetValue(message.ToLogin, out var target) == false)
            {
                var resp = new Response(false, "Target not found", loggedIn.Account.Balance);

                await socket.SendMessage(resp);

                return;
            }

            if(loggedIn.TryTransfer(target, message.Amount))
            {
                var resp = new Response(false, "Not enough funds", loggedIn.Account.Balance);

                await socket.SendMessage(resp);

                return;
            }

            var response = new Response(true, "Transfer Succeded", loggedIn.Account.Balance);

            await socket.SendMessage(response);
        }
    }
}