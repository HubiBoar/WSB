using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;
using static ToDoApp.HtmxHelper;

namespace ToDoApp;

public static partial class Methods
{
    public sealed record UpdateTodoCommand(int Id, int Pos, bool Value);

    public static IResult UpdateTodo([FromBody] UpdateTodoCommand command, Todos todos)
    {
        var todo = todos.FirstOrDefault(x => x.Id == command.Id);
        if (todo != null)
        {
            todos.Remove(todo);
            todos.Insert(command.Pos, todo with { Done = command.Value });
        }

        return todos.Select(RenderTodo).ToIResult();
    }
}