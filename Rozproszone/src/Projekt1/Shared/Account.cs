namespace Shared;

public sealed partial class Account
{
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public int Balance { get; private set; }

    internal string Login { get; }
    internal string Password { get; }

    public Account(string name, string surname, string login, string password)
    {
        Name = name;
        Surname = surname;
        Login = login;
        Password = password;
    }

    public void EditUserData(string name, string surname)
    {
        Name    = name;
        Surname = surname;
    }

    public void Deposit(int amount)
    {
        Balance += amount;
    }
}
