using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp;

public static partial class Methods
{
    public static class Update
    {
        public sealed record Command(int Id, int Pos, bool Value);

        private static readonly string Path = "/update-todo";
        public static readonly HtmlAttribute Htmx = ToDoApp.Htmx.Put(Path);

        public static void Register(WebApplication app) =>
            app.MapPut(Path, Method)
                .WithOpenApi();

        private static IResult Method([FromBody] Command command, Todos todos)
        {
            Console.WriteLine("Update");

            var todo = todos.FirstOrDefault(x => x.Id == command.Id);
            if (todo != null)
            {
                todos.Remove(todo);
                todos.Insert(command.Pos, todo with { Done = command.Value });
            }

            return todos.Select(Render.Todo).ToIResult();
        }
    }
}