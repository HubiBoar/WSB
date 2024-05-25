using System.Net.Sockets;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    internal delegate Task<bool> Method(Socket server, Sockets.Message message);

    internal static async Task OnServer(params Method[] methods)
    {
        using var server = await Sockets.CreateServer();

        while (true)
        {
            var message = await server.RecieveMessage();

            foreach(var method in methods)
            {
                if(await method(server, message))
                {
                    break;
                }
            }
        }
    }
}
