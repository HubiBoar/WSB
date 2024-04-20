using System.Security.Claims;
using System.Text.Json;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HyperTextExpression.HtmlExp;

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

    public static void Map(WebApplication app)
    {
        app
            .MapGroup("identity/")
            .MapIdentityApi<User>();

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
    }

    public static class LoginPage
    {
        public static class Element
        {
            public const string LoginInput = "input-login";
            public const string PasswordInput = "input-password";
        }

        public static IResult Render() => HtmlDoc(
            Head(
                ("title", "Login page!")
            ),
            Body(
                Div(
                    Attrs("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                    Div(
                        HtmlEl("form",
                            ("h4", "Login or Register"),
                            ("input",
                                Attrs(
                                    ("id", Element.LoginInput),
                                    ("type", "text"),
                                    ("name", "email")
                                )
                            ),
                            ("input",
                                Attrs(
                                    ("id", Element.PasswordInput),
                                    ("type", "password"),
                                    ("name", "password")
                                )
                            ),
                            ("button",
                                Attrs(
                                    Htmx.Post("/identity/login"),
                                    Htmx.Ext("json-enc"),
                                    Htmx.OnAfterRequest($"(document.getElementById('{Element.LoginInput}').value = '')"),
                                    Htmx.OnAfterRequest($"(document.getElementById('{Element.PasswordInput}').value = '')")
                                ),
                                "Login"),
                            ("button",
                                Attrs(
                                    Htmx.Post("/identity/register"),
                                    Htmx.Ext("json-enc"),
                                    Htmx.OnAfterRequest($"(document.getElementById('{Element.LoginInput}').value = '')"),
                                    Htmx.OnAfterRequest($"(document.getElementById('{Element.PasswordInput}').value = '')")
                                ),
                                "Register")
                        )
                    )
                ),
                Htmx.HtmxScript,
                Htmx.HtmxJsonEncScript
            )
        ).ToIResult();
    }
}