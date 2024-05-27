//Server
using Server.Public;

var dataBase = Server.Account.DataBase.CreateWithTestUsers();

await Logic.Server.Run
(
    new Logic.Login.OnServer(dataBase),
    new Logic.Info.OnServer(dataBase),
    new Logic.Deposit.OnServer(dataBase),
    new Logic.Withdraw.OnServer(dataBase),
    new Logic.Transfer.OnServer(dataBase),
    new Logic.EditInfo.OnServer(dataBase),
    new Logic.Admin.EditInfo.OnServer(dataBase)
);