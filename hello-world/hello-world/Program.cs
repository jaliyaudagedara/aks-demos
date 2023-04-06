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

app.MapGet("/", (JsonSerializerOptions jsonSerializerOptions) =>
{
    HelloWorldMessage helloWorldMessage = new()
    {
        Message = "Hello World!",
        OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
        FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
        HostName = Dns.GetHostName()
    };

    return Results.Json(helloWorldMessage, jsonSerializerOptions);
})
.WithName("HelloWorld")
.WithOpenApi();

app.Run();

internal record HelloWorldMessage()
{
    public string Message { get; set; }

    public string OSDescription { get; set; }

    public string FrameworkDescription { get; set; }

    public string HostName { get; set; }
}
