//Client
using Shared;
using Server.Public;

Console.WriteLine("Client :: Hello, World!");

using var client = await Sockets.CreateClient();

var token = await Logic.Login.OnClient(client);
    

while(true)
{
    Console.WriteLine("----------------------------------------------------------------------------");

    Console.WriteLine("Select commands:");
    Console.WriteLine("Info");
    Console.WriteLine("Deposit");
    Console.WriteLine("Withdraw");
    Console.WriteLine("Transfer");
    Console.WriteLine("EditInfo");

    Console.WriteLine();
    var command = Console.ReadLine()!;

    if(command == "Info")
    {
        var account = await Logic.GetInfo.OnClient(client, token);

        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine($"Name: {account.Name}");
        Console.WriteLine($"Surname: {account.Surname}");
        Console.WriteLine($"Balance: {account.Balance}");
    }

    if(command.StartsWith("Deposit"))
    {
        Console.WriteLine();
        Console.WriteLine("<--[Input]-->");
        Console.WriteLine();

        Console.WriteLine("Amount:");
        var amount = int.Parse(Console.ReadLine()!);

        var balance = await Logic.Deposit.OnClient(client, token, amount);


        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine($"Balance: {balance}");
    }

    if(command.StartsWith("Withdraw"))
    {
        Console.WriteLine("<--[Input]-->");
        Console.WriteLine("Amount:");
        var amount = int.Parse(Console.ReadLine()!);

        var (success, balance) = await Logic.Withdraw.OnClient(client, token, amount);


        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine($"Success: {success}");
        Console.WriteLine($"Balance: {balance}");
    }

    if(command.StartsWith("Transfer"))
    {
        Console.WriteLine();
        Console.WriteLine("<--[Input]-->");
        Console.WriteLine();

        Console.WriteLine("To:");
        var to = Console.ReadLine()!;

        Console.WriteLine("Amount:");
        var amount = int.Parse(Console.ReadLine()!);

        var (success, balance) = await Logic.Transfer.OnClient(client, token, to, amount);

        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine($"Success: {success}");
        Console.WriteLine($"Balance: {balance}");
    }

    if(command.StartsWith("EditInfo"))
    {
        Console.WriteLine();
        Console.WriteLine("<--[Input]-->");
        Console.WriteLine();

        Console.WriteLine("Name:");
        var name = Console.ReadLine()!;

        Console.WriteLine("Surname:");
        var surname = Console.ReadLine()!;

        var account = await Logic.EditInfo.OnClient(client, token, name, surname);

        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine($"Name: {account.Name}");
        Console.WriteLine($"Surname: {account.Surname}");
        Console.WriteLine($"Balance: {account.Balance}");
    }

    Console.WriteLine();
    Console.WriteLine("Coninue?");
    Console.ReadLine();
}
