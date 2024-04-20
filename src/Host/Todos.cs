namespace ToDoApp;

public sealed record Todo(int Id, string Deed, bool Done);

public sealed class Todos : List<Todo>
{
    private int _idCount = 0;

    public static IServiceCollection Register(IServiceCollection services) => services.AddSingleton<Todos>();

    public Todos()
    {
        Add("Educate the masses.", true);
        Add("Fix HTML in .NET", true);
        Add("Learn HTMX");
        Add("Change underwear");
    }

    public void Add(string deed, bool done = false) => Add(new Todo(_idCount++, deed, done));
}