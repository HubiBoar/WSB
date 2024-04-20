using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;
using static ToDoApp.Htmx;

namespace ToDoApp;

public static partial class Methods
{
    public static class Render
    {
        public static void Register(WebApplication app)
        {
            Console.WriteLine("Get");

            app.MapGet("/", Method)
            .WithOpenApi();
        }

        private static IResult Method(Todos todos) => HtmlDoc(
            Head(
                ("title", "Todo App!")
            ),
            Body(
                Div(
                    Attrs("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                    ("h1", "My to do's"),
                    Div(
                        Attrs("id", "todo-list"),
                        todos.Select(Todo)
                    ),
                    Div(
                        HtmlEl("form",
                            ("h4", "Add Todo"),
                            ("input",
                                Attrs(
                                    ("id", "todo-input"),
                                    ("type", "text"),
                                    ("name", "deed")
                                )
                            ),
                            ("button",
                                Attrs(
                                    Add.Htmx,
                                    ("hx-ext", "json-enc"),
                                    ("hx-target", "#todo-list"),
                                    ("hx-on", "htmx:afterRequest: (document.getElementById('todo-input').value = '')")
                                ),
                                "add")
                        )
                    )
                ),
                HtmxScript,
                HtmxJsonEncScript
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
                        ("hx-ext", "json-enc"),
                        ("hx-target", "#todo-list")
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
                        ("hx-target", "#todo-list")
                    ),
                    "Delete"
                )
            );
    }
}