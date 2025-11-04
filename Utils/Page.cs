using System.Globalization;
using Scriban;
using Scriban.Runtime;
using System.Net.Mime;
using System.Text;

namespace Portfolio.Utils;

class HtmlResult(string html) : IResult
{
    private readonly string _html = html;

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}

public class Page(TemplateContext context)
{
    public async Task<IResult> Html(string pagePath, object? values = null)
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import(values);
        context.PushGlobal(scriptObject);
        context.PushCulture(new CultureInfo("pt-BR"));

        var pageFile = await File.ReadAllTextAsync($"Pages/{pagePath}.html");

        var htmlString = Template.Parse(pageFile).Render(context);

        return new HtmlResult(htmlString);
    }
}