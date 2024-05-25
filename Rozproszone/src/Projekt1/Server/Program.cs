//Server
using Shared;

Console.WriteLine("Server :: Hello, World!");

var accounts = Account.DataBase.CreateWithTestUsers();

using var server = await Sockets.CreateServer();

while (true)
{
    var message = await server.RecieveMessage();

    var response = message.Command switch
    {
        Sockets.Command.Start => Sockets.Message.StartOk,
        _                     => Sockets.Message.Unknown,
    };

    await server.SendMessage(response);

    break;
}

server.ShutdownBoth();