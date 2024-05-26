using Shared;

namespace Server;

internal sealed partial class Account
{
    public sealed partial class LoggedIn
    {
        public Sockets.Token LoginToken { get; }

        public Account Account { get; }

        private LoggedIn(Account account, Sockets.Token loginToken)
        {
            Account = account;
            LoginToken = loginToken;
        }

        public static (bool status, string message) TryLogin
        (
            DataBase accounts,
            Sockets.Token token,
            out LoggedIn loggedIn
        )
        {
            return TryLogin(accounts, token.Login, token.Password, out loggedIn);
        }

        public static (bool status, string message) TryLogin
        (
            DataBase accounts,
            string login,
            string password,
            out LoggedIn loggedIn
        )
        {
            loggedIn = null!;

            if(accounts.Accounts.TryGetValue(login, out var account) == false)
            {
                return (false, $"No Account with login : '{login}' found");
            }

            if(account.Password != password)
            {
                return (false, $"Invalid password for login : '{login}'");
            }

            loggedIn = new LoggedIn(account, new Sockets.Token(login, password, account.IsAdmin));

            return (true, $"Succesfully logged in");
        }

        public bool TryWithdraw(int amount)
        {
            if(amount < 0)
            {
                return false;
            }

            if(amount <= Account.Balance)
            {
                Account.Balance -= amount;
                return true;
            }

            return false;
        }

        public bool TryTransfer(Account target, int amount)
        {
            if(amount < 0)
            {
                return false;
            }

            if(TryWithdraw(amount))
            {
                target.Deposit(amount);
                return true;
            }

            return false;
        }
    }
}
