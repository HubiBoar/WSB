using System.Text;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    public static partial class Admin
    {
        public static class EditInfo
        {
            public sealed record Request(Sockets.Token Token, string ToLogin, string Name, string Surname) : Sockets.ITokenMessage
            {
                public static string Command => "AdminEditInfo";
            }

            private sealed record Response(bool Success, string Message, AccountDTO Account) : Sockets.IMessage
            {
                public static string Command => "AdminEditInfoResp";
            }

            public sealed class OnClient : Client.IAdminHandle
            {
                public string ConsoleCommand => "AdminEditInfo";

                public async Task<StringBuilder> Handle
                (
                    Sockets.Handler socket,
                    Sockets.Token token
                )
                {
                    Print.Input();

                    var who = Print.ReadValue<string>("Who:")!;
                    var name = Print.ReadValue<string>("Name:")!;
                    var surname = Print.ReadValue<string>("Surname:")!;

                    var (success, responseMessage, account) = await Handle(socket, token, who, name, surname);

                    if(success == false)
                    {
                        return new StringBuilder($"Failure: '{responseMessage}'"); 
                    }

                    var (newName, newSurname, balance, _) = account;

                    return new StringBuilder()
                        .AppendLine($"Target: {who}")
                        .AppendLine($"Name: {newName}")
                        .AppendLine($"Surname: {newSurname}")
                        .AppendLine($"Balance: {balance}");
                }

                private static async Task<Response> Handle
                (
                    Sockets.Handler socket,
                    Sockets.Token token,
                    string who,
                    string name,
                    string surname
                )
                {
                    var message = new Request(token, who, name, surname);

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

                    if(loggedIn.Account.IsAdmin == false)
                    {
                        var resp = new Response(false, "User is not an Admin", null!);

                        await socket.SendMessage(resp);
                    }

                    if(_dataBase.Accounts.TryGetValue(message.ToLogin, out var target) == false)
                    {
                        var resp = new Response(false, "Target not found", null!);

                        await socket.SendMessage(resp);

                        return;
                    }

                    target.EditUserData(message.Name, message.Surname);

                    var response = new Response(true, "Edit Succeded", new AccountDTO(target));

                    await socket.SendMessage(response);
                }
            }
        }
    }
}