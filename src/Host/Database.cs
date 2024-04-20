using Microsoft.EntityFrameworkCore;

public static class DataBase
{
    public static void Sqlite(DbContextOptionsBuilder builder) => builder.UseSqlite("DataSource=app.db");
}