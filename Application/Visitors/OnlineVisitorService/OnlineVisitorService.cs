using Application.Contexts.Mongo;
using Domain.Visitors;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Visitors.OnlineVisitorService;

public class OnlineVisitorService : IOnlineVisitorService
{
    #region Constructor

    private readonly IMongoDbContext<OnlineVisitor> _context;
    private readonly IMongoCollection<OnlineVisitor> _onlineVisitors;

    public OnlineVisitorService(IMongoDbContext<OnlineVisitor> context)
    {
        _context = context;
        _onlineVisitors = _context.GetCollection();
    }

    #endregion

    public async Task Add(string clientId)
    {
        var isVisitorExists = await _onlineVisitors.AsQueryable().AnyAsync(x => x.ClientId == clientId);
        if (isVisitorExists)
        {
            return;
        }

        var visitor = new OnlineVisitor
        {
            ClientId = clientId,
            VisitTime = DateTime.UtcNow
        };

        await _onlineVisitors.InsertOneAsync(visitor);
    }

    public async Task<long> GetOnlineVisitorsCount()
    {
        return await _onlineVisitors.AsQueryable().LongCountAsync();
    }

    public async Task Remove(string clientId)
    {
        await _onlineVisitors.FindOneAndDeleteAsync(x => x.ClientId == clientId);
    }
}