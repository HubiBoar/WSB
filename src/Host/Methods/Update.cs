using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp;

public static partial class Methods
{
    public sealed record UpdateTodoCommand(int Id, int Pos, bool Value);

    private static readonly string UpdateTodoPath = "/update-todo";
    private static readonly HtmlAttribute UpdateTodoHtmx = Htmx.Put(UpdateTodoPath);

    public static void RegisterUpdate(WebApplication app) =>
         app.MapPut(UpdateTodoPath, Methods.UpdateTodoMethod)
            .WithOpenApi();

    private static IResult UpdateTodoMethod([FromBody] UpdateTodoCommand command, Todos todos)
    {
        Console.WriteLine("Update");

        var todo = todos.FirstOrDefault(x => x.Id == command.Id);
        if (todo != null)
        {
            todos.Remove(todo);
            todos.Insert(command.Pos, todo with { Done = command.Value });
        }

        return todos.Select(RenderTodo).ToIResult();
    }
}