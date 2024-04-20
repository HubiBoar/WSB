using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

//dotnet ef migrations add TodosRecords --context Todo+Context --output-dir Migrations/Todo
//dotnet ef database update --context Todo+Context
public static partial class Todo
{
    public static async Task<IReadOnlyCollection<Record>> GetTodos(this ClaimsPrincipal user, Context context)
    {
        var username = user.Identity!.Name;
        return await context.Todos
            .Where(x => x.UserName == username)
            .OrderBy(x => x.CreationDate)
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public static async Task AddTodo(this ClaimsPrincipal user, Context context, string deed, bool done)
    {
        var username = user.Identity!.Name!;

        await context.Todos.AddAsync(new RecordDataModel(username, deed, done));

        await context.SaveChangesAsync();
    }

    public static async Task RemoveTodo(this ClaimsPrincipal user, Context context, string id)
    {
        var record = await context.Todos.FirstAsync(x => x.Id == id);

        context.Todos.Remove(record);

        await context.SaveChangesAsync();
    }

    public static async Task UpdateTodo(this ClaimsPrincipal user, Context context, string id, bool done)
    {
        var record = await context.Todos.FirstAsync(x => x.Id == id);

        context.Todos.Update(record with {
            Done = done
        });

        await context.SaveChangesAsync();
    }

    public sealed record Record(string Id, string Deed, bool Done);

    public sealed record RecordDataModel(string UserName, string Deed, bool Done)
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public Record ToDto() => new (Id, Deed, Done);
    }

    public sealed class Context : DbContext
    {
        public DbSet<RecordDataModel> Todos { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordDataModel>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<RecordDataModel>()
                .HasData([
                    new ("hubibubi@gmail.com", "Clean house", true),
                    new ("hubibubi@gmail.com", "Buy stuff",   true),
                    new ("hubibubi@gmail.com", "Go to gym",   false),
                    new ("hubibubi@gmail.com", "Eat",         false)
                ]);
        }
    }
}