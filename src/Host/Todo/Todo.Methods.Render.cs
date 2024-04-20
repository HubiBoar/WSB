using System.Security.Claims;
using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;

public static partial class Todo
{
    public static class Render
    {
        public static class Element
        {
            public const string List  = "todo-list";
            public const string Input = "todo-input";
        }

        public static void Map(WebApplication app) => app
            .MapGet("/", async (ClaimsPrincipal user, Context context) => 
            {
                Console.WriteLine("Get");
                var todos = await user.GetTodos(context);
                return Method(user.Identity!.Name!, todos);
            })
            .WithOpenApi()
            .RequireAuthorization();

        private static IResult Method(string username, IReadOnlyCollection<Record> todos) => HtmlDoc(
            Head(
                ("title", "Todo App!")
            ),
            Body(
                Div(
                    Attrs("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                    ("h2", $"ToDo list for {username}"),
                    Div(
                        Attrs("id", Element.List),
                        todos.Select(RenderTodo)
                    ),
                    Div(
                        HtmlEl("form",
                            ("h4", "Add Todo"),
                            ("input",
                                Attrs(
                                    ("id", Element.Input),
                                    ("type", "text"),
                                    ("name", "deed")
                                )
                            ),
                            ("button",
                                Attrs(
                                    Add.Html,
                                    Htmx.Ext("json-enc"),
                                    Htmx.TargetHash(Element.List),
                                    Htmx.OnAfterRequest($"(document.getElementById('{Element.Input}').value = '')")
                                ),
                                "add")
                        )
                    )
                ),
                Htmx.HtmxScript,
                Htmx.HtmxJsonEncScript
            )
        ).ToIResult();

        public static HtmlEl RenderTodo(Record todo, int index) =>
            HtmlEl("form",
                ("input",
                    Attrs(
                        ("type", "checkbox"),
                        todo.Done ? "checked" : "",
                        Update.Html,
                        Htmx.Ext("json-enc"),
                        Htmx.TargetHash(Element.List)
                    )
                ),
                ("input", Attrs(("type", "hidden"), ("name", "id"), ("value", todo.Id))),
                ("input", Attrs(("type", "hidden"), ("name", "value"), ("value", !todo.Done))),
                ("input", Attrs(("type", "hidden"), ("name", "pos"), ("value", index))),
                ("label",
                    Attrs("style", todo.Done ? "text-decoration: line-through;" : ""),
                    todo.Deed
                ),
                ("a",
                    Attrs(
                        Delete.Html(todo),
                        Htmx.TargetHash(Element.List)
                    ),
                    "Delete"
                )
            );
    }
}