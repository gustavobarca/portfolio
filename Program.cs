using Portfolio.Utils;
using Scriban;
using Scriban.Runtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITemplateLoader, TemplateLoader>();
builder.Services.AddScoped(provider => new TemplateContext { TemplateLoader = provider.GetRequiredService<ITemplateLoader>() });
builder.Services.AddScoped<Page>();

var app = builder.Build();

app.MapGet("/", (Page page) => page.Html("home"));
app.UseStaticFiles();

app.Run();