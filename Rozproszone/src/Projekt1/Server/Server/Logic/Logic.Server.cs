using System.Text.Json;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    internal static class Server
    {
        public interface IHandle
        {
            string Command { get; }

            Task Handle(Sockets.Handler handler, object message);
        }

        public interface IHandle<T> : IHandle
            where T : Sockets.IMessage
        {
            string IHandle.Command => T.Command;

            Task IHandle.Handle(Sockets.Handler socket, object message)
            {
                var json = JsonSerializer.Serialize(message);
                var casted = JsonSerializer.Deserialize<T>(json);

                return Handle(socket, casted!);
            }

            Task Handle(Sockets.Handler socket, T message);
        }

        public static async Task Run(params IHandle[] handlers)
        {
            using var server = await Sockets.CreateServer();

            while (true)
            {
                try
                {
                    var socket = await server.Socket.AcceptAsync();
                    Console.WriteLine("New connection accepted!");
                    _ = HandleConnectionAsync(new Sockets.Handler(socket, server.Debug));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting connection: {ex.Message}");
                }
            }

            async Task HandleConnectionAsync(Sockets.Handler socket)
            {
                while (true)
                {
                    try
                    {
                        var (success, command, payload) = await socket.RecieveMessage();

                        if(success == Sockets.Status.Disconnected)
                        {
                            Console.WriteLine($"Disconnected");

                            socket.Dispose();
                            return;
                        }

                        if(success == Sockets.Status.SerializationIssue)
                        {
                            continue;
                        }

                        foreach(var handler in handlers)
                        {
                            if(handler.Command == command)
                            {
                                try
                                {
                                    await handler.Handle(socket, payload);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
        }
    }
}
