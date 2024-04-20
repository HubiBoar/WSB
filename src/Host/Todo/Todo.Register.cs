using Microsoft.EntityFrameworkCore;

public static partial class Todo
{
    public static void Register(IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions)
    {
        BoolConverter.Register(services);

        services.AddDbContext<Context>(dbOptions);
    }

    public static void Map(WebApplication app)
    {
        List.Map(app);
        Add.Map(app);
        Update.Map(app);
        Delete.Map(app);

        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<Context>();
    
        context.Database.EnsureCreated();
    }
}