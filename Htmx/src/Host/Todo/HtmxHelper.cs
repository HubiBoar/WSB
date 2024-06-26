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

    public static HtmlEl BootstrapCSS = (
        "link",
        Attrs("href", """https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous""")
    );

    public static HtmlAttribute Get(string path)    => ("hx-get", path);
    public static HtmlAttribute Put(string path)    => ("hx-put", path);
    public static HtmlAttribute Post(string path)   => ("hx-post", path);
    public static HtmlAttribute Delete(string path) => ("hx-delete", path);

    public static HtmlAttribute On(string value)             => ("hx-on", value);
    public static HtmlAttribute Ext(string value)            => ("hx-ext", value);
    public static HtmlAttribute Target(string value)         => ("hx-target", value);
    public static HtmlAttribute TargetHash(string value)     => ("hx-target", $"#{value}");
    public static HtmlAttribute OnAfterRequest(string value) => ("hx-on", $"htmx:afterRequest: {value}");

}