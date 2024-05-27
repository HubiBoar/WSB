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
                Print.Input();

                var to = Print.ReadValue<string>("To:");
                var amount = Print.ReadValue<int>("Amount:");

                var (success, message, balance) = await Handle(socket, token, to, amount);

                return new StringBuilder()
                    .AppendLine(success ? "Success" : $"Failed: {message}")
                    .AppendLine($"Balance: {balance}");
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

        internal sealed class OnServer : Server.IHandle<Request>
        {
            private readonly Account.DataBase _dataBase;

            public OnServer(Account.DataBase dataBase)
            {
                _dataBase = dataBase;
            }

            public async Task Handle(Sockets.Handler socket, Request message)
            {
                Account.LoggedIn.TryLogin(_dataBase, message.Token, out var loggedIn);

                if(_dataBase.Accounts.TryGetValue(message.ToLogin, out var target) == false)
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
}