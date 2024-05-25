//Server
using Server.Public;

Console.WriteLine("Server :: Hello, World!");

var dataBase = Server.Account.DataBase.CreateWithTestUsers();

await Logic.OnServer
(
    (server, message) => Logic.Login.OnServer(server, message, dataBase),
    (server, message) => Logic.GetInfo.OnServer(server, message, dataBase),
    (server, message) => Logic.Deposit.OnServer(server, message, dataBase),
    (server, message) => Logic.Withdraw.OnServer(server, message, dataBase),
    (server, message) => Logic.Transfer.OnServer(server, message, dataBase),
    (server, message) => Logic.EditInfo.OnServer(server, message, dataBase)
);