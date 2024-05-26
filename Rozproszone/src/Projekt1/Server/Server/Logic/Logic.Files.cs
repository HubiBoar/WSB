namespace Server.Public;

public sealed record Account(string Name, string Surname, int Balance, bool IsAdmin)
{
    internal Account(Server.Account account) : this(account.Name, account.Surname, account.Balance, account.IsAdmin) {}
}