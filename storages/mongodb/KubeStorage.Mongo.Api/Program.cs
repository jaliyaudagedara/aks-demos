using KubeStorage.Mongo.Api.Models;
using KubeStorage.Mongo.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CustomerDatabaseSettings>(builder.Configuration.GetSection("CustomersDatabase"));
builder.Services.AddSingleton<CustomersService>();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/customers/{id}", async Task<Results<Ok<Customer>, NotFound>> (CustomersService customersService, string id) =>
{
    Customer? customer = await customersService.GetAsync(id);
    if (customer == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(customer);
});

app.MapGet("/customers", async (CustomersService customersService) =>
{
    return TypedResults.Ok(await customersService.GetAsync());
});

app.MapPost("/customers", async (CustomersService customersService, Customer customer) =>
{
    await customersService.CreateAsync(customer);

    return TypedResults.Created($"/customers/{customer.Id}", customer);
});

app.Run();
