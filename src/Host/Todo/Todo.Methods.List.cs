using System.Security.Claims;
using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;

public static partial class Todo
{
    public static class List
    {
        public static class Element
        {
            public const string List  = "todo-list";
            public const string Input = "todo-input";
        }

        private readonly static string Path = "/todo/list";

        public static void Map(WebApplication app) => app
            .MapGet(Path, Render)
            .WithOpenApi()
            .RequireAuthorization();

        public static async Task<IResult> Render(ClaimsPrincipal user, Context context)
        {
            Console.WriteLine("List");

            var todos = await user.GetTodos(context);
            return Page(user.Identity!.Name!, todos).ToIResult();
        }

        private static HtmlEl Page(string username, IReadOnlyCollection<Record> todos) => HtmlDoc(
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
        );

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