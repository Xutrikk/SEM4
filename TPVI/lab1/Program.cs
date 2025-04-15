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
        // ������� ������ ��� ��������� ����������
        var builder = WebApplication.CreateBuilder(args);

        // ��������� ������ HTTP Logging
        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All; // ���������� ��� ���������, ������� � ������
        });

        // ������ ����������
        var app = builder.Build();

        // �������� HTTP Logging ����� ���������� ��������
        app.UseHttpLogging();

        // ���������� ������� ��� ��������� �������
        app.MapGet("/", () => "��� ������ ASPA � ������������ HTTP-��������");
        app.MapGet("/asd", () => "��� ������ ASPA 52");

        // ��������� ����������
        app.Run();
    }
}
