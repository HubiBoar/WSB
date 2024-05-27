//Client
using Server.Public;

await Logic.Client.Run
(
    new Logic.Login.OnClient(),
    new Logic.Info.OnClient(),
    new Logic.EditInfo.OnClient(),
    new Logic.Deposit.OnClient(),
    new Logic.Withdraw.OnClient(),
    new Logic.Transfer.OnClient(),
    new Logic.Admin.EditInfo.OnClient()
);