using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Shared;

public static partial class Sockets
{
    public static Task Client(Message firstMessage, Func<Message, (Message? Response, bool Shutdown)> loop)
    {
        return Client(firstMessage, (msg) => Task.FromResult(loop(msg)));
    }

    public static Task Client(Func<Message, (Message? Response, bool Shutdown)> loop)
    {
        return Client((msg) => Task.FromResult(loop(msg)));
    }

    public static Task Server(Func<Message, (Message? Response, bool Shutdown)> loop)
    {
        return Server((msg) => Task.FromResult(loop(msg)));
    }

    public static async Task Client(Func<Message, Task<(Message? Response, bool Shutdown)>> loop)
    {
        using var client = await CreateClient();

        await Loop(client, null, loop);
    }

    public static async Task Client(Message firstMessage, Func<Message, Task<(Message? Response, bool Shutdown)>> loop)
    {
        using var client = await CreateClient();

        await Loop(client, firstMessage, loop);
    }

    public static async Task Server(Func<Message, Task<(Message? Response, bool Shutdown)>> loop)
    {
        using var server = await CreateServer();

        await Loop(server, null, loop);
    }

    private static async Task Loop(Socket client, Message? firstMessage, Func<Message, Task<(Message? Response, bool Shutdown)>> loop)
    {
        if(firstMessage is not null)
        {
            await client.SendMessage(firstMessage);
        }

        while (true)
        {
            var message = await client.RecieveMessage();

            var (response, shutdown) = await loop(message);

            if(response is not null)
            {
                await client.SendMessage(response);
            }

            if(shutdown)
            {
                break;
            }
        }

        client.Shutdown(SocketShutdown.Both);
    }

    private static async Task<Socket> CreateClient()
    {
        var (client, endpoint) = await CreateSocket();

        await client.ConnectAsync(endpoint);

        return client;
    }

    private static async Task<Socket> CreateServer()
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

    private static async Task<Message> RecieveMessage(this Socket client)
    {
        var buffer    = new byte[1_024];
        var received  = await client.ReceiveAsync(buffer, SocketFlags.None);
        var decodeded = Encoding.UTF8.GetString(buffer, 0, received);

        var splitMessage = decodeded.Split("]");
        var command      = splitMessage[0].Replace("[", "");
        var payload      = splitMessage[1];

        Console.WriteLine($"Message Rciv :: '[{command}]' :: '{payload}'");

        return new Message(Enum.Parse<Command>(command), payload);
    }

    private static async Task SendMessage(this Socket socket, Message message)
    {
        var command  = $"[{Enum.GetName(message.Command)}]";
        var formated = $"{command}{message.Payload}";
        var encoded  = Encoding.UTF8.GetBytes(formated);

        Console.WriteLine($"Message Send :: '{command}' :: '{message.Payload}'");

        await socket.SendAsync(encoded, 0);
    }
}