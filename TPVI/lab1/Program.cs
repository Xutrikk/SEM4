using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpLogging;

internal class Program
{
    private static void Main(string[] args)
    {
        // Создаем билдер для настройки приложения
        var builder = WebApplication.CreateBuilder(args);

        // Добавляем сервис HTTP Logging
        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All; // Логировать все заголовки, запросы и ответы
        });

        // Строим приложение
        var app = builder.Build();

        // Включаем HTTP Logging перед обработкой запросов
        app.UseHttpLogging();

        // Определяем маршрут для корневого запроса
        app.MapGet("/", () => "Мое первое ASPA с логированием HTTP-запросов");
        app.MapGet("/asd", () => "Мое первое ASPA 52");

        // Запускаем приложение
        app.Run();
    }
}
