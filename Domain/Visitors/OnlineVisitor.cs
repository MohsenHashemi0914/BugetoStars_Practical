using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Visitors;

public class OnlineVisitor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string ClientId { get; set; }
    public DateTime VisitTime { get; set; }
}