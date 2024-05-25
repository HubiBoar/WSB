using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Shared;

public static partial class Sockets
{
    public static async Task<Socket> CreateClient()
    {
        var (client, endpoint) = await CreateSocket();

        await client.ConnectAsync(endpoint);

        return client;
    }

    public static async Task<Socket> CreateServer()
    {
        var (server, endpoint) = await CreateSocket();

        server.Bind(endpoint);
        server.Listen(100);

        var handler = await server.AcceptAsync();

        return handler;
    }

    private static async Task<(Socket Client, IPEndPoint Endpoint)> CreateSocket()
    {
        var hostName  = Dns.GetHostName();

        var localhost = await Dns.GetHostEntryAsync(hostName);
        
        var address   = localhost.AddressList[0];
        var endpoint  = new IPEndPoint(address, 11_000);

        var client    = new Socket
        (
            endpoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        return (client, endpoint);
    }

    public static async Task<Message> RecieveMessage(this Socket client)
    {
        var buffer    = new byte[1_024];
        var received  = await client.ReceiveAsync(buffer, SocketFlags.None);
        var decodeded = Encoding.UTF8.GetString(buffer, 0, received);

        var splitMessage = decodeded.Split("]");
        var command      = splitMessage[0].Replace("[", "");
        var payload      = splitMessage[1];

        Console.WriteLine($"Message Rciv :: '[{command}]' :: '{payload}'");

        return new Message(command, payload);
    }

    public static async Task SendMessage(this Socket socket, Message message)
    {
        var command  = $"[{message.Command}]";
        var formated = $"{command}{message.Payload}";
        var encoded  = Encoding.UTF8.GetBytes(formated);

        Console.WriteLine($"Message Send :: '{command}' :: '{message.Payload}'");

        await socket.SendAsync(encoded, 0);
    }

    public static void ShutdownBoth(this Socket socket)
    {
        socket.Shutdown(SocketShutdown.Both);
    }
}