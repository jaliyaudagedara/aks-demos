using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KubeStorage.Mongo.Api.Models;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
