//Client
using Server.Public;

await Logic.Client.Run(
    new Logic.Login.OnClient(),
    new Logic.GetInfo.OnClient(),
    new Logic.EditInfo.OnClient(),
    new Logic.Deposit.OnClient(),
    new Logic.Withdraw.OnClient(),
    new Logic.Transfer.OnClient());