using System.Security.Claims;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static HyperTextExpression.HtmlExp;


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

        app.MapPost("identity/logout", async (SignInManager<User> signInManager, HttpContext context) => 
        {
            await signInManager.SignOutAsync();

            context.Response.Headers.Append("HX-Refresh", "true");

            return Results.Ok();
        });

        app.MapGet("/", async (ClaimsPrincipal user, Todo.Context context) =>
        {
            if(user?.Identity?.Name is null)
            {
                Console.WriteLine("Need to login");

                return LoginPage.Render();
            }
            else
            {
                Console.WriteLine("LoggedIn");

                return await Todo.List.Render(user, context);
            } 
        })
        .WithOpenApi()
        .AllowAnonymous();

        app.MapGet("/refresh", (HttpContext context) =>
        {
            context.Response.Headers.Append("HX-Refresh", "true");

            return Results.Ok();
        })
        .WithOpenApi()
        .AllowAnonymous();

        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<Context>();
    
        context.Database.EnsureCreated();
    }

    public static class LoginPage
    {
        public static class Element
        {
            public const string LoginInput = "input-login";
            public const string PasswordInput = "input-password";
            public const string CheckInput = "input-check";
        }

        public static IResult Render() => HtmlDoc(
            Head
            (
                ("title", "Login page!")
            ),
            Body
            (
                Div
                (
                    Attrs
                    (
                        ("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                        ("class", "card bg-light")
                    ),
                    Div
                    (
                        Attrs
                        (
                            ("class", "card-body")
                        ),
                        HtmlEl
                        ("form",
                            Div
                            (
                                Attrs
                                (
                                    ("class", "mb-3")
                                ),
                                ("label",
                                    Attrs
                                    (
                                        ("for", Element.LoginInput),
                                        ("class", "form-label")
                                    ),
                                    "Email Address"),
                                ("input",
                                    Attrs
                                    (
                                        ("id", Element.LoginInput),
                                        ("type", "email"),
                                        ("name", "email"),
                                        ("class", "form-control"),
                                        ("aria-describedby", "emailHelp")
                                    )
                                ),
                                Div
                                (
                                    Attrs
                                    (
                                        ("id", "emailHelp"),
                                        ("class", "form-text")
                                    ),
                                    "hubibubi@gmail.com"
                                )
                            ),
                            Div
                            (
                                Attrs
                                (
                                    ("class", "mb-3")
                                ),
                                ("label",
                                    Attrs
                                    (
                                        ("for", Element.PasswordInput),
                                        ("class", "form-label")
                                    ),
                                    "Password"),
                                ("input",
                                    Attrs
                                    (
                                        ("id", Element.PasswordInput),
                                        ("type", "password"),
                                        ("name", "password"),
                                        ("class", "form-control")
                                    )
                                ),
                                Div
                                (
                                    Attrs
                                    (
                                        ("id", "passwordHelp"),
                                        ("class", "form-text")
                                    ),
                                    "Test!1"
                                )
                            ),
                            ("button",
                                Attrs
                                (
                                    ("type", "submit"),
                                    ("class", "btn btn-success"),
                                    Htmx.Post("/identity/login"),
                                    Htmx.Ext("json-enc"),
                                    Htmx.OnAfterRequest($"htmx.trigger('#refresh','click')")
                                ),
                                "Login"
                            ),
                            ("button",
                                Attrs
                                (
                                    ("type", "submit"),
                                    ("class", "btn btn-primary"),
                                    Htmx.Post("/identity/register"),
                                    Htmx.Ext("json-enc"),
                                    Htmx.OnAfterRequest($"htmx.trigger('#refresh','click')")
                                ),
                                "Register"
                            ),
                            Div
                            (
                                Attrs
                                (
                                    ("id", "refresh"),
                                    ("hx-trigger", "click"),
                                    Htmx.Get("/refresh")
                                )
                            )
                        )
                    )
                ),
                Htmx.HtmxScript,
                Htmx.HtmxJsonEncScript,
                Htmx.BootstrapCSS
            )
        ).ToIResult();
    }
}