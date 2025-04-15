var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/aspnetcore", subApp =>
{
    subApp.UseWelcomePage();
});
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
