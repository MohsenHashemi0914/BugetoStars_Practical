using Application.Contexts.Mongo;
using Application.Visitors.SaveVisitorInfo.Dtos;
using Domain.Visitors;
using MongoDB.Driver;

namespace Application.Visitors.SaveVisitorInfo;

public class SaveVisitorInfoService : ISaveVisitorInfoService
{
    #region Constructor

    private readonly IMongoDbContext<Visitor> _mongoDbContext;

    public SaveVisitorInfoService(IMongoDbContext<Visitor> mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _mongoCollection = _mongoDbContext.GetCollection();
    }

    private readonly IMongoCollection<Visitor> _mongoCollection;

    #endregion

    public void Execute(SaveVisitorInfoDto command)
    {
        var visitor = new Visitor
        {
            IP = command.IP,
            Method = command.Method,
            Protocol = command.Protocol,
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

        _mongoCollection.InsertOne(visitor);
    }
}