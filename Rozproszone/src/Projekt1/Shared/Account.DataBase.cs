namespace Shared;

public sealed partial class Account
{
    public sealed class DataBase
    {
        public IReadOnlyCollection<Account> Accounts => _accounts;
        private readonly List<Account> _accounts;

        private DataBase()
        {
            _accounts = new List<Account>();
        }

        public static DataBase CreateWithTestUsers()
        {
            var db = new DataBase();

            var testAccount = new Account("TestName", "TestSurname", "Test", "test");

            db.AddAccount(testAccount);

            return db;
        }

        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }
    }
}