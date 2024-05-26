using System.Net.Sockets;
using System.Text.Json;
using Shared;

namespace Server.Public;

public static partial class Logic
{

    public static class EditInfo
    {
        public sealed record Request(Sockets.Token Token, string Name, string Surname) : Sockets.ITokenMessage
        {
            public static string Command => "EditInfo";
        }

        private sealed record Response(Account Account) : Sockets.IMessage
        {
            public static string Command => "EditInfoResp";
        }

        public static async Task<Account> OnClient
        (
            Sockets.Handler socket,
            Sockets.Token token,
            string name,
            string surname
        )
        {
            var message = new Request(token, name, surname);

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

            loggedIn.Account.EditUserData(message.Name, message.Surname);

            var response = new Response(new (loggedIn.Account));

            await socket.SendMessage(response);
        }
    }
}