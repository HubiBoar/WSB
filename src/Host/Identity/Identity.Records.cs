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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var user = new User
            {
                Id = "hubibubi@gmail.com",
                Email = "hubibubi@gmail.com",
                EmailConfirmed = true, 
                UserName = "hubibubi@gmail.com",
                NormalizedUserName = "HUBIBUBI@GMAIL.COM"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Test!1");

            builder.Entity<User>().HasData([user]);

            base.OnModelCreating(builder);
        }
    }
}