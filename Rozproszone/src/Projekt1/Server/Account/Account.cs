namespace Server;

internal sealed partial class Account
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int Balance { get; private set; }
    public bool IsAdmin { get; private set; }

    internal string Login { get; }
    internal string Password { get; }

    private Account
    (
        string name,
        string surname,
        string login,
        string password,
        int balance,
        bool isAdmin
    )
    {
        Name = name;
        Surname = surname;
        Login = login;
        Password = password;
        Balance = balance;
        IsAdmin = isAdmin;
    }

    public void EditUserData(string name, string surname)
    {
        Name    = name;
        Surname = surname;
    }

    public void Deposit(int amount)
    {
        if(amount < 0)
        {
            return;
        }

        Balance += amount;
    }
}
