using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp;

public static partial class Methods
{
    public static class Add
    {
        public sealed record Command(string Deed);

        private static readonly string Path = "/add-todo";
        public static readonly HtmlAttribute Htmx = ToDoApp.Htmx.Post(Path);

        public static void Register(WebApplication app) =>
            app.MapPost(Path, Method)
            .WithOpenApi();

        private static IResult Method([FromBody] Command command, Todos todos)
        {
            Console.WriteLine("Add");

            todos.Add(command.Deed);
            return todos.Select(Render.Todo).ToIResult();
        }
    }
}