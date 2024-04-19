using HyperTextExpression;
using HyperTextExpression.AspNetCore;
using static HyperTextExpression.HtmlExp;
using static ToDoApp.HtmxHelper;

namespace ToDoApp;

public static partial class Methods
{
    public static IResult Render(Todos todos) => HtmlDoc(
        Head(
            ("title", "Todo App!")
        ),
        Body(
            Div(
                Attrs("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                ("h1", "My to do's"),
                Div(
                    Attrs("id", "todo-list"),
                    todos.Select(RenderTodo)
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
                                "hx-post", "/todo-app/add-todo",
                                "hx-ext", "json-enc",
                                "hx-target", "#todo-list",
                                "hx-on", "htmx:afterRequest: (document.getElementById('todo-input').value = '')"
                            ),
                            "add")
                    )
                )
            ),
            HtmxScript,
            HtmxJsonEncScript
        )
    ).ToIResult();

    private static HtmlEl RenderTodo(Todo todo, int index) =>
        HtmlEl("form",
            ("span", Children(
                ("strong", $"{todo.Id}.")
            )),
            ("input",
                Attrs(
                    ("type", "checkbox"),
                    todo.Done ? "checked" : "",
                    ("hx-put", "/todo-app/update-status"),
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
                    "hx-delete", $"/todo-app/delete-todo/{todo.Id}",
                    "hx-target", "#todo-list"
                ),
                "Delete"
            )
        );
}