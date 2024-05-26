using Shared;

namespace Server.Public;

public static partial class Logic
{
    internal static Method GetMethod<T>(Func<Sockets.Handler, T, Task> func)
        where T : Sockets.IMessage
    {
        return new Method(T.Command, (handler, msg) => 
        {
            return func(handler, (T)msg);
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
                var message = await handler.RecieveMessage();

                foreach(var method in methods)
                {
                    if(method.Command == message.Command)
                    {
                       await method.Func(handler, message.Payload);
                       break; 
                    }
                }
            }
        }
    }
}
