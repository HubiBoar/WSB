namespace Server;

internal sealed partial class Account
{
    public sealed class DataBase
    {
        public IReadOnlyDictionary<string, Account> Accounts => _accounts;
        private readonly Dictionary<string, Account> _accounts;

        private DataBase()
        {
            _accounts = new ();
        }

        public static DataBase CreateWithTestUsers()
        {
            var db = new DataBase();

            db.TryAddAccount("User1 Name", "User1 Surname", "User1", "user", 10, false);
            db.TryAddAccount("User2 Name", "User2 Surname", "User2", "user", 50, false);
            db.TryAddAccount("Admin Name", "Admin Surname", "Admin", "admin", 50, true);

            return db;
        }

        public bool TryAddAccount(string name, string surname, string login, string password, int balance, bool isAdmin)
        {
            return _accounts.TryAdd(login, new Account(name, surname, login, password, balance, isAdmin));
        }
    }
}