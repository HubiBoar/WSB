using HyperTextExpression;
using static HyperTextExpression.HtmlExp;

namespace ToDoApp;

public static class Htmx
{
    public static HtmlEl HtmxScript = (
        "script",
        Attrs("src", "https://unpkg.com/htmx.org@1.9.4",
            "integrity", "sha384-zUfuhFKKZCbHTY6aRR46gxiqszMk5tcHjsVFxnUo8VMus4kHGVdIYVbOYYNlKmHV",
            "crossorigin", "anonymous"
        )
    );

    public static HtmlEl HtmxJsonEncScript = (
        "script",
        Attrs("src", "https://unpkg.com/htmx.org/dist/ext/json-enc.js")
    );

    public static HtmlAttribute Get(string path)    => ("hx-get", path);
    public static HtmlAttribute Put(string path)    => ("hx-put", path);
    public static HtmlAttribute Post(string path)   => ("hx-post", path);
    public static HtmlAttribute Delete(string path) => ("hx-delete", path);
}