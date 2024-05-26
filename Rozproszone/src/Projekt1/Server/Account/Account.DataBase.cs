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

            db.TryAddAccount("Name1", "Surname1", "User1", "test", 10, false);
            db.TryAddAccount("Name2", "Surname2", "User2", "test", 50, false);
            db.TryAddAccount("AdminAccount", "AdminSurname", "Admin", "admin", 50, true);

            return db;
        }

        public bool TryAddAccount(string name, string surname, string login, string password, int balance, bool isAdmin)
        {
            return _accounts.TryAdd(login, new Account(name, surname, login, password, balance, isAdmin));
        }
    }
}