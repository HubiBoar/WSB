namespace Server.Public;

public sealed record AccountDTO(string Name, string Surname, int Balance, bool IsAdmin)
{
    internal AccountDTO(Server.Account account) : this(account.Name, account.Surname, account.Balance, account.IsAdmin) {}
}