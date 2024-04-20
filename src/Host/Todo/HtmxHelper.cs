using HyperTextExpression;
using static HyperTextExpression.HtmlExp;

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

    public static HtmlAttribute Ext(string value)    => ("hx-ext", value);
    public static HtmlAttribute Target(string value)    => ("hx-target", value);
    public static HtmlAttribute TargetHash(string value)    => ("hx-target", $"#{value}");
    public static HtmlAttribute On(string value)    => ("hx-on", value);
    public static HtmlAttribute OnAfterRequest(string value)    => ("hx-on", $"htmx:afterRequest: {value}");

    public static HtmlAttribute Get(string path)    => ("hx-get", path);
    public static HtmlAttribute Put(string path)    => ("hx-put", path);
    public static HtmlAttribute Post(string path)   => ("hx-post", path);
    public static HtmlAttribute Delete(string path) => ("hx-delete", path);
}