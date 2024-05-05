using static HyperTextExpression.HtmlExp;

namespace Tests;

public class Main
{
    [Fact]
    public void base_html_doc()
    {
        var html = HtmlDoc();
        var htmlString = html.ToString();

        Assert.Equal(html.ToString(), htmlString);
    }
}