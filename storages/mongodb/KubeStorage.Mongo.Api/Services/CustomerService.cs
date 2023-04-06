using KubeStorage.Mongo.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace KubeStorage.Mongo.Api.Services;

public class CustomersService
{
    private readonly IMongoCollection<Customer> _booksCollection;

    public CustomersService(IOptions<CustomerDatabaseSettings> bookStoreDatabaseSettings)
    {
        MongoClient mongoClient = new(bookStoreDatabaseSettings.Value.ConnectionString);
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
        _booksCollection = mongoDatabase.GetCollection<Customer>(bookStoreDatabaseSettings.Value.CustomersCollectionName);
    }

    public async Task<List<Customer>> GetAsync(CancellationToken cancellationToken = default) =>
        await _booksCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Customer?> GetAsync(string id, CancellationToken cancellationToken = default) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken = default) =>
        await _booksCollection.InsertOneAsync(customer, cancellationToken: cancellationToken);

    public async Task UpdateAsync(string id, Customer customer, CancellationToken cancellationToken = default) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, customer, cancellationToken: cancellationToken);

    public async Task RemoveAsync(string id, CancellationToken cancellationToken = default) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
}

