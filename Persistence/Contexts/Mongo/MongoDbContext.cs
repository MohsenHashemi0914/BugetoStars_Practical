using Application.Contexts.Mongo;
using MongoDB.Driver;

namespace Persistence.Contexts.Mongo;

public class MongoDbContext<T> : IMongoDbContext<T>
{
    #region Constructor

    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<T> _mongoCollection;

    public MongoDbContext()
    {
        var client = new MongoClient();
        _db = client.GetDatabase("MongoDb");
        _mongoCollection = _db.GetCollection<T>(typeof(T).Name);
    }

    #endregion

    public IMongoCollection<T> GetCollection()
    {
        return _mongoCollection;
    }
}