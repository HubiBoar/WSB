using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Shared;

public static partial class Sockets
{
    public sealed record Handler(Socket Socket, bool Debug) : IDisposable
    {
        public void Dispose()
        {
            Socket.Dispose();
        }
    }

    public static async Task<Handler> CreateClient(bool debug = false)
    {
        var (client, endpoint) = await CreateSocket();

        await client.ConnectAsync(endpoint);

        return new (client, debug);
    }

    public static async Task<Handler> CreateServer(bool debug = true)
    {
        var (server, endpoint) = await CreateSocket();

        server.Bind(endpoint);
        server.Listen(100);

        return new (server, debug);
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

    private sealed record MessageCommand(string Command);
    private sealed record MessageCommandPayload(string Command, object Payload);

    private sealed record MessageCommand<TMessage>(string Command, TMessage Payload)
        where TMessage : IMessage;

    public static async Task<TMessage> TryRecieveMessage<TMessage>(this Handler client)
        where TMessage : class, IMessage
    {
        try
        {
            while(true)
            {
                var buffer    = new byte[1_024];
                var received  = await client.Socket.ReceiveAsync(buffer, SocketFlags.None);
                var decodeded = Encoding.UTF8.GetString(buffer, 0, received);

                var message = JsonSerializer.Deserialize<MessageCommand>(decodeded)!;

                if(client.Debug)
                {
                    Console.WriteLine($"Message Rciv :: '{decodeded}'");
                }

                if(message.Command != TMessage.Command)
                {
                    continue;
                }

                var deserialized = JsonSerializer.Deserialize<MessageCommand<TMessage>>(decodeded)!;

                return deserialized.Payload;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public static async Task<(string Command, object Payload)> RecieveMessage(this Handler client)
    {
        try
        {
            var buffer    = new byte[1_024];
            var received  = await client.Socket.ReceiveAsync(buffer, SocketFlags.None);
            var decodeded = Encoding.UTF8.GetString(buffer, 0, received);

            var message = JsonSerializer.Deserialize<MessageCommandPayload>(decodeded)!;

            if(client.Debug)
            {
                Console.WriteLine($"Message Rciv :: '{decodeded}'");
            }

            return (message.Command, message.Payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public static async Task SendMessage<TMessage>(this Handler socket, TMessage message)
        where TMessage : class, IMessage
    {
        var msg = new MessageCommand<TMessage>(TMessage.Command, message);
        var json = JsonSerializer.Serialize(msg)!;

        var encoded  = Encoding.UTF8.GetBytes(json);

        if(socket.Debug)
        {
            Console.WriteLine($"Message Send :: '{json}'");
        }

        await socket.Socket.SendAsync(encoded, 0);
    }
}