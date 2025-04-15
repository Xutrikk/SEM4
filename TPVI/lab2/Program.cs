using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Фильтр сообщений логирования
builder.Logging.AddFilter("Microsoft.AspNetCore.Diagnostics", LogLevel.None);

var app = builder.Build();

// 🔹 Глобальный обработчик ошибок (редирект на "/error")
app.UseExceptionHandler("/error");

// 🔹 Обычная конечная точка
app.MapGet("/", () => "Start");

// 🔹 Конечная точка, вызывающая пользовательское исключение
app.MapGet("/test1", () =>
{
    throw new Exception("-- Exception Test --"); // Искусственное исключение
});

// 🔹 Конечная точка, вызывающая деление на ноль
app.MapGet("/test2", () =>
{
    int x = 0, y = 5, z = 0;
    z = y / x; // Ошибка: DivideByZeroException
    return "test2";
});

// 🔹 Конечная точка, выходящая за границы массива
app.MapGet("/test3", () =>
{
    int[] x = new int[3] { 1, 2, 3 };
    int y = x[3]; // Ошибка: IndexOutOfRangeException
    return "test3";
});

// 🔹 Конечная точка для обработки ошибок
app.Map("/error", async (ILogger<Program> logger, HttpContext context) =>
{
    IExceptionHandlerFeature? exobj = context.Features.Get<IExceptionHandlerFeature>(); // Получаем информацию об исключении
    await context.Response.WriteAsync($"<h1>Oops!</h1>"); // Сообщение пользователю
    logger.LogError(exobj?.Error, "ExceptionHandler"); // Логирование ошибки
});

app.Run();
