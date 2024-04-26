using System.Security.Claims;
using HyperTextExpression;
using HyperTextExpression.AspNetCore;

public static partial class Todo
{
    public static class Delete
    {
        private static readonly string Path = "/todo/delete";
        public static HtmlAttribute Html(Record todo) => Htmx.Delete($"{Path}/{todo.Id}");

        public static void Map(WebApplication app) => app
            .MapDelete(Path + "/{id}", Render)
            .WithOpenApi()
            .RequireAuthorization();

        private static async Task<IResult> Render(string id, ClaimsPrincipal user, Context context)
        {
            Console.WriteLine("Delete");

            await user.RemoveTodo(context, id);

            var todos = await user.GetTodos(context);

            return todos.Select(List.RenderTodo).ToIResult();
        }
    }
}