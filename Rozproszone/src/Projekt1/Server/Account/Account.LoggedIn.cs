namespace Server;

internal sealed partial class Account
{
    public sealed partial class LoggedIn
    {
        public Token LoginToken { get; }

        public Account Account { get; }

        private LoggedIn(Account account, Token loginToken)
        {
            Account = account;
            LoginToken = loginToken;
        }

        public static (bool status, string message) TryLogin(DataBase accounts, Token token, out LoggedIn loggedIn)
        {
            loggedIn = null!;
            var (login, password) = token;

            if(accounts.Accounts.TryGetValue(login, out var account) == false)
            {
                return (false, $"No Account with login : '{login}' found");
            }

            if(account.Password != password)
            {
                return (false, $"Invalid password for login : '{login}'");
            }

            loggedIn = new LoggedIn(account, token);

            return (true, $"Succesfully logged in");
        }

        public bool TryWithdraw(int amount)
        {
            if(amount <= Account.Balance)
            {
                Account.Balance -= amount;
                return true;
            }

            return false;
        }

        public bool TryTransfer(Account target, int amount)
        {
            if(TryWithdraw(amount))
            {
                target.Deposit(amount);
                return true;
            }

            return false;
        }
    }
}
