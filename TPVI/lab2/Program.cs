using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("Neumann.html");

app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Picture")),
    RequestPath = "/static"
});

app.MapGet("/aspnetcore", () => "Добро пожаловать в ASP.NET Core!");

app.Run();
