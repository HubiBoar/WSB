using HyperTextExpression;
using HyperTextExpression.AspNetCore;

namespace ToDoApp;

public static partial class Methods
{
    private static readonly string DeleteTodoPath = "/delete-todo";
    private static HtmlAttribute DeleteTodoHtmx(Todo todo) => Htmx.Delete($"{DeleteTodoPath}/{todo.Id}");

    public static void RegisterDelete(WebApplication app) =>
        app.MapDelete(DeleteTodoPath, Methods.DeleteTodo)
           .WithOpenApi();

    public static IResult DeleteTodo(int id, Todos todos)
    {
        Console.WriteLine("Add");

        var todo = todos.FirstOrDefault(x => x.Id == id);
        if (todo != null)
        {
            todos.Remove(todo);
        }

        return todos.Select(RenderTodo).ToIResult();
    }
}