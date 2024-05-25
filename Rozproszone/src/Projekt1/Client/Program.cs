//Client
using Shared;

Console.WriteLine("Hello, World!");

await Sockets.Client
(
    Sockets.Message.Start,
    (message) => 
    {
        var response = message.Command switch
        {
            Sockets.Command.StartOk => null,
            _                       => Sockets.Message.Unknown,
        };

        return (response, Shutdown: true);
    }
);