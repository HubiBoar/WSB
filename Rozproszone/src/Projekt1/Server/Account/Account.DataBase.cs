namespace Server;

internal sealed partial class Account
{
    public sealed class DataBase
    {
        public IReadOnlyDictionary<string, Account> Accounts => _accounts;
        private readonly Dictionary<string, Account> _accounts;

        private DataBase()
        {
            _accounts = new Dictionary<string, Account>();
        }

        public static DataBase CreateWithTestUsers()
        {
            var db = new DataBase();

            db.TryAddAccount("TestName", "TestSurname", "Test", "test", 10);
            db.TryAddAccount("Test2Name", "Test2Surname", "Test2", "test", 50);

            return db;
        }

        public bool TryAddAccount(string name, string surname, string login, string password, int balance)
        {
            return _accounts.TryAdd(login, new Account(name, surname, login, password, balance));
        }
    }
}