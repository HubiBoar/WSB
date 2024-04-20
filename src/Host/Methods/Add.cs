using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp;

public static partial class Methods
{
    public sealed record AddTodoCommand(string Deed);

    private static readonly string AddTodoPath = "/add-todo";
    private static readonly HtmlAttribute AddTodoHtmx = Htmx.Post(AddTodoPath);

    public static void RegisterAdd(WebApplication app) =>
        app.MapPost(AddTodoPath, AddTodo)
           .WithOpenApi();

    private static IResult AddTodo([FromBody] AddTodoCommand command, Todos todos)
    {
        Console.WriteLine("Add");

        todos.Add(command.Deed);
        return todos.Select(RenderTodo).ToIResult();
    }
}