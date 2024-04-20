using HyperTextExpression;
using HyperTextExpression.AspNetCore;

namespace ToDoApp;

public static partial class Methods
{
    public static class Delete
    {
        private static readonly string Path = "/delete-todo";
        public static HtmlAttribute Htmx(Todo todo) => ToDoApp.Htmx.Delete($"{Path}/{todo.Id}");

        public static void Register(WebApplication app) =>
            app.MapDelete(Path + "/{id:int}", Method)
            .WithOpenApi();

        private static IResult Method(int id, Todos todos)
        {
            var todo = todos.FirstOrDefault(x => x.Id == id);
            if (todo != null)
            {
                todos.Remove(todo);
            }

            return todos.Select(Render.Todo).ToIResult();
        }
    }
}