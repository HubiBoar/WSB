using Microsoft.EntityFrameworkCore;

public static class DataBase
{
    public static Action<DbContextOptionsBuilder> Get(IConfiguration configuration, string inMemoryName) => 
        configuration["DataBase"] == "Sqlite"
        ?
        DataBase.Sqlite
        :
        builder => DataBase.InMemory(builder, inMemoryName);

    public static void Sqlite(DbContextOptionsBuilder builder) => builder.UseSqlite("DataSource=app.db");

    public static void InMemory(DbContextOptionsBuilder builder, string name) => builder.UseInMemoryDatabase(name);
}