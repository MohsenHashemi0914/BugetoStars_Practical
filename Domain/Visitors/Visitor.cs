using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Visitors;

public class Visitor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string IP { get; set; }
    public string Method { get; set; }
    public DateTime Time { get; set; }
    public string Protocol { get; set; }
    public string VisitorId { get; set; }
    public string CurrentLink { get; set; }
    public string ReferrerLink { get; set; }
    public string PhysicalPath { get; set; }

    public virtual Device Device { get; set; }
    public virtual VisitorVersion Browser { get; set; }
    public virtual VisitorVersion OperationSystem { get; set; }
}