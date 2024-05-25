//Server
using Shared;

Console.WriteLine("Server :: Hello, World!");

await Sockets.Server((message) => 
{
    var response = message.Command switch
    {
        Sockets.Command.Start => Sockets.Message.StartOk,
        _                     => Sockets.Message.Unknown,
    };

    return (response, Shutdown: true);
});