using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public static class DataBase
{
    public static Action<DbContextOptionsBuilder> Get(IConfiguration configuration, string inMemoryName) => (builder) => InMemory(builder, inMemoryName);

    public static void Sqlite(DbContextOptionsBuilder builder) => builder.UseSqlite("DataSource=app.db");

    public static void InMemory(DbContextOptionsBuilder builder, string name) => builder.UseInMemoryDatabase(name);
}

//dotnet ef migrations add IdentityRecords --context Identity+Context --output-dir Migrations/Identity
//dotnet ef database update --context Identity+Context
public static partial class Identity
{    
    public sealed class CustomSignInManager : SignInManager<User>
    {
        public CustomSignInManager(
            UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task SignOutAsync()
        {    
            await Context.SignOutAsync(IdentityConstants.BearerScheme);
        }
    }


    public sealed class User : IdentityUser
    {
    }

    public static void Register(IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions)
    {
        services.AddAuthentication()
            .AddCookie(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();

        services.AddDbContext<Context>(dbOptions);

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<Context>()
            .AddApiEndpoints()
            .AddSignInManager<CustomSignInManager>();
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGroup("identity/")
            .MapIdentityApi<User>();
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