using System.Security.Claims;
using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

public static partial class Todo
{
    public static class Update
    {
        public sealed record Command(string Id, bool Value);

        private static readonly string Path = "/update-todo";
        public static readonly HtmlAttribute Html = Htmx.Put(Path);

        public static void Map(WebApplication app) => app
            .MapPut(Path, Method)
            .WithOpenApi()
            .RequireAuthorization();

        private static async Task<IResult> Method([FromBody] Command command, ClaimsPrincipal user, Context context)
        {
            Console.WriteLine("Update");

            await user.UpdateTodo(context, command.Id, command.Value);

            var todos = await user.GetTodos(context);

            return todos.Select(Render.RenderTodo).ToIResult();
        }
    }
}