using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;

namespace ToDoApp;

public static partial class Methods
{
    public static class Render
    {
        public static class Element
        {
            public const string List  = "todo-list";
            public const string Input = "todo-input";
        }

        public static void Register(WebApplication app) =>
            app.MapGet("/", (Todos todos) => 
                {
                    Console.WriteLine("Get");
                    return Method(todos);
                })
                .WithOpenApi();

        private static IResult Method(Todos todos) => HtmlDoc(
            Head(
                ("title", "Todo App!")
            ),
            Body(
                Div(
                    Attrs("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                    ("h1", "My to do's"),
                    Div(
                        Attrs("id", $"{Element.List}"),
                        todos.Select(Todo)
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
                                    Add.Htmx,
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

        public static HtmlEl Todo(Todo todo, int index) =>
            HtmlEl("form",
                ("span", Children(
                    ("strong", $"{todo.Id}.")
                )),
                ("input",
                    Attrs(
                        ("type", "checkbox"),
                        todo.Done ? "checked" : "",
                        Update.Htmx,
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
                        Delete.Htmx(todo),
                        Htmx.TargetHash(Element.List)
                    ),
                    "Delete"
                )
            );
    }
}