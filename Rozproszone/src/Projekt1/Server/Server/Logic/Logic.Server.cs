using System.Diagnostics;
using System.Text.Json;
using Shared;

namespace Server.Public;

public static partial class Logic
{
    internal static Method GetMethod<T>(Func<Sockets.Handler, T, Task> func)
        where T : Sockets.IMessage
    {
        return new Method(T.Command, (handler, msg) => 
        {
            Console.WriteLine(T.Command + "RUNNING");
            var json = JsonSerializer.Serialize(msg);
            var casted = JsonSerializer.Deserialize<T>(json);
            Console.WriteLine(T.Command + "Casted");

            return func(handler, casted!);
        });
    }

    internal sealed record Method(string Command, Func<Sockets.Handler, object, Task> Func);

    internal static async Task OnServer(params Method[] methods)
    {
        using var server = await Sockets.CreateServer();

        while (true)
        {
            try
            {
                var handler = await server.Socket.AcceptAsync();
                Console.WriteLine("New connection accepted!");
                _ = HandleConnectionAsync(new Sockets.Handler(handler, server.Debug));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accepting connection: {ex.Message}");
            }
        }

        async Task HandleConnectionAsync(Sockets.Handler handler)
        {
            while (true)
            {
                try
                {
                    var (command, payload) = await handler.RecieveMessage();

                    foreach(var method in methods)
                    {
                        Console.WriteLine(method.Command);
                        Console.WriteLine(payload);
                        if(method.Command == command)
                        {
                            try
                            {
                                await method.Func(handler, payload);
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
