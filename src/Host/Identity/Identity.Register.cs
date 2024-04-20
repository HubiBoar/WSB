using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static partial class Identity
{
    public static void Register(IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions)
    {
        services.AddAuthentication()
            .AddCookie(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();

        services.AddDbContext<Context>(dbOptions);

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<Context>()
            .AddApiEndpoints();
    }

    public static void Map(WebApplication app) => app
        .MapGroup("identity/")
        .MapIdentityApi<User>();
}