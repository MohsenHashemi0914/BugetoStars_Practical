using Application.Contexts.Mongo;
using Application.Visitors.SaveVisitorInfo.Dtos;
using Domain.Visitors;
using MongoDB.Driver;

namespace Application.Visitors.SaveVisitorInfo;

public class SaveVisitorInfoService : ISaveVisitorInfoService
{
    #region Constructor

    private readonly IMongoDbContext<Visitor> _mongoDbContext;
    private readonly IMongoCollection<Visitor> _mongoCollection;

    public SaveVisitorInfoService(IMongoDbContext<Visitor> mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _mongoCollection = _mongoDbContext.GetCollection();
    }

    #endregion

    public async Task ExecuteAsync(SaveVisitorInfoDto command)
    {
        var visitor = new Visitor
        {
            IP = command.IP,
            Time = DateTime.UtcNow,
            Method = command.Method,
            Protocol = command.Protocol,
            VisitorId = command.VisitorId,
            CurrentLink = command.CurrentLink,
            ReferrerLink = command.ReferrerLink,
            PhysicalPath = command.PhysicalPath,
            Device = new()
            {
                Brand = command.Device.Brand,
                Model = command.Device.Model,
                Family = command.Device.Family,
                IsSpider = command.Device.IsSpider
            },
            Browser = new()
            {
                Family = command.Browser.Family,
                Version = command.Browser.Version
            },
            OperationSystem = new()
            {
                Family = command.OperationSystem.Family,
                Version = command.OperationSystem.Version
            }
        };

        await _mongoCollection.InsertOneAsync(visitor);
    }
}