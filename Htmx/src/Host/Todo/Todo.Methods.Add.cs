using System.Security.Claims;
using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

public static partial class Todo
{
    public static class Add
    {
        public sealed record Command(string Deed);

        private static readonly string Path = "/todo/add";
        public static readonly HtmlAttribute Html = Htmx.Post(Path);

        public static void Map(WebApplication app) => app
            .MapPost(Path, Render)
            .WithOpenApi()
            .RequireAuthorization();

        private static async Task<IResult> Render([FromBody] Command command, ClaimsPrincipal user, Context context)
        {
            Console.WriteLine("Add");

            await user.AddTodo(context, command.Deed, false);

            var todos = await user.GetTodos(context);

            return todos.Select(List.RenderTodo).ToIResult();
        }
    }
}