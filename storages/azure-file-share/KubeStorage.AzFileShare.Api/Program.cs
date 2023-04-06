using System.Text;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

var dataPath = Path.Combine("files", "azure", "customers.json");

app.MapGet("/customers", () =>
{
    List<Customer>? customers = GetCustomers();

    return customers;
});

app.MapPost("/customers", (Customer customer) =>
{
    List<Customer>? customers = GetCustomers();
    customers.Add(customer);

    string jsonString = JsonSerializer.Serialize(customers, new JsonSerializerOptions()
    {
        WriteIndented = true
    });

    using StreamWriter streamWriter = new(dataPath);
    streamWriter.Write(jsonString);

    return customer;
});

app.Run();

List<Customer> GetCustomers()
{
    if (!File.Exists(dataPath))
    {
        using FileStream fileStream = File.Create(dataPath);
        byte[] info = new UTF8Encoding(true).GetBytes("[ ]");
        fileStream.Write(info, 0, info.Length);
    }

    using StreamReader streamReader = new(dataPath);
    string json = streamReader.ReadToEnd();
    return JsonSerializer.Deserialize<List<Customer>>(json)!;
}

internal record Customer(string FirstName, string LastName);