using System.Net;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new JsonSerializerOptions
{
    WriteIndented = true,
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (IConfiguration configuration, JsonSerializerOptions jsonSerializerOptions) =>
{
    string? serviceName = configuration.GetValue<string>("ServiceName");

    HelloWorldMessage helloWorldMessage = new()
    {
        Message = string.IsNullOrEmpty(serviceName) ? "Hello World!" : $"Hello World from {serviceName}!",
        OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
        FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
        HostName = Dns.GetHostName()
    };

    return Results.Json(helloWorldMessage, jsonSerializerOptions);
})
.WithName("HelloWorld")
.WithOpenApi();

app.MapGet("/fibonacci", (int number) =>
{
    return Fibonacci(number);
})
.WithName("GetFibonacciNumber")
.WithOpenApi();

app.Run();

int Fibonacci(int n)
{
    if (n == 0)
    {
        return 0;
    }
    else if (n == 1)
    {
        return 1;
    }

    return Fibonacci(n - 1) + Fibonacci(n - 2);
}

internal record HelloWorldMessage()
{
    public string Message { get; set; }

    public string OSDescription { get; set; }

    public string FrameworkDescription { get; set; }

    public string HostName { get; set; }
}
