using MongoDB.Driver;

namespace Application.Contexts.Mongo;

public interface IMongoDbContext<T>
{
    IMongoCollection<T> GetCollection();
}