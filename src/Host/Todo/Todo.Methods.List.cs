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

            public static string ButtonId(int index) => $"button-id-{index}";
            public static string ButtonValue(int index) => $"button-value-{index}";
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
            Head
            (
                ("title", "Todo App!")
            ),
            Body
            (
                Div
                (
                    Attrs
                    (
                        ("style", "max-width: 800px; margin: auto; margin-bottom: 5rem;"),
                        ("class", "card bg-light")
                    ),
                    Div
                    (
                        Attrs
                        (
                            ("class", "card-body")
                        ),
                        ("h2", $"ToDo list for {username}"),
                        ("button",
                            Attrs
                            (
                                ("class", "btn btn-danger btn-sm"),
                                Htmx.Post("/identity/logout"),
                                Htmx.Ext("json-enc"),
                                Htmx.OnAfterRequest($"htmx.trigger('#refresh','click')")
                            ),
                            "Logout"
                        ),
                        // Div
                        // (
                        //     Attrs
                        //     (
                        //         ("id", "refresh"),
                        //         ("hx-trigger", "click"),
                        //         Htmx.Get("/refresh")
                        //     )
                        // ),
                        Div
                        (
                            Attrs
                            (
                                ("id", Element.List),
                                ("class", "list-group")
                            ),
                            todos.Select(RenderTodo)
                        ),
                        Div
                        (
                            HtmlEl
                            ("form",
                                ("h4", "Add Todo"),
                                ("input",
                                    Attrs
                                    (
                                        ("id", Element.Input),
                                        ("type", "text"),
                                        ("name", "deed")
                                    )
                                ),
                                ("button",
                                    Attrs
                                    (
                                        ("type", "submit"),
                                        ("class", "btn btn-success btn-sm"),
                                        Add.Html,
                                        Htmx.Ext("json-enc"),
                                        Htmx.TargetHash(Element.List),
                                        Htmx.OnAfterRequest($"(document.getElementById('{Element.Input}').value = '')")
                                    ),
                                    "Add"
                                )
                            )
                        )
                    )
                ),
                Htmx.HtmxScript,
                Htmx.HtmxJsonEncScript,
                Htmx.BootstrapCSS
            )
        );

        public static HtmlEl RenderTodo(Record todo, int index) =>
            Div
            (
                Attrs
                (
                    ("class", "list-group-item")
                ),
                Div
                (
                    Attrs
                    (
                        ("class", "btn-group d-flex"),
                        ("role", "group")
                    ),
                        ("button",
                            Attrs
                            (
                                ("class", todo.Done ? "btn btn-primary w-100" : "btn btn-outline-primary w-100"),
                                ("type", "button"),
                                Update.Html,
                                ("hx-include", $"#{Element.ButtonId(index)}, #{Element.ButtonValue(index)}"),
                                Htmx.Ext("json-enc"),
                                Htmx.TargetHash(Element.List)
                            ),
                                todo.Deed
                        ),
                        ("button",
                            Attrs
                            (
                                ("class", "btn btn-danger w-50"),
                                ("type", "button"),
                                ("hx-include", $"#{Element.ButtonId(index)}"),
                                Delete.Html(todo),
                                Htmx.TargetHash(Element.List)
                            ),
                                "Delete"
                        ),
                        ("input",
                            Attrs
                            (
                                ("type", "hidden"),
                                ("id", Element.ButtonId(index)),
                                ("name", "id"),
                                ("value", todo.Id)
                            )
                        ),
                        ("input",
                            Attrs
                            (
                                ("type", "hidden"),
                                ("id", Element.ButtonValue(index)),
                                ("name", "value"),
                                ("value", !todo.Done)
                            )
                        )
                )
            );
    }
}