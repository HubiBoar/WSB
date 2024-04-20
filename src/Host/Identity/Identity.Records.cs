using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//dotnet ef migrations add IdentityRecords --context Identity+Context --output-dir Migrations/Identity
//dotnet ef database update --context Identity+Context
public static partial class Identity
{    
    public sealed class User : IdentityUser
    {
    }

    public sealed class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
    }
}