//Client
using Shared;

Console.WriteLine("Client :: Hello, World!");

using var client = await Sockets.CreateClient();

while (true)
{
    await client.SendMessage(Sockets.Message.Start);

    var message = await client.RecieveMessage();

    if(message.Command == Sockets.Command.StartOk)
    {
        break;
    }
}

client.ShutdownBoth();