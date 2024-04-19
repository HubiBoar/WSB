using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;
using static ToDoApp.HtmxHelper;

namespace ToDoApp;

public static partial class Methods
{
    public sealed record AddTodoCommand(string Deed);

    public sealed static IResult AddTodo([FromBody] AddTodoCommand command, Todos todos)
    {
        todos.Add(command.Deed);
        return todos.Select(RenderTodo).ToIResult();
    }
}