//Server
using Server.Public;

Console.WriteLine("Server :: Hello, World!");

var dataBase = Server.Account.DataBase.CreateWithTestUsers();

await Logic.OnServer
(
    Logic.GetMethod<Logic.Login.Request>((server, message)    => Logic.Login.OnServer(server, message, dataBase)),
    Logic.GetMethod<Logic.GetInfo.Request>((server, message)  => Logic.GetInfo.OnServer(server, message, dataBase)),
    Logic.GetMethod<Logic.Deposit.Request>((server, message)  => Logic.Deposit.OnServer(server, message, dataBase)),
    Logic.GetMethod<Logic.Withdraw.Request>((server, message) => Logic.Withdraw.OnServer(server, message, dataBase)),
    Logic.GetMethod<Logic.Transfer.Request>((server, message) => Logic.Transfer.OnServer(server, message, dataBase)),
    Logic.GetMethod<Logic.EditInfo.Request>((server, message) => Logic.EditInfo.OnServer(server, message, dataBase))
);