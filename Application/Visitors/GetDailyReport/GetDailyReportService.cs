using Application.Contexts.Mongo;
using Application.Visitors.GetDailyReport.Dtos;
using Domain.Visitors;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Visitors.GetDailyReport;

public class GetDailyReportService : IGetDailyReportService
{
    #region Constructor

    private readonly IMongoDbContext<Visitor> _mongoDbContext;
    private readonly IMongoCollection<Visitor> _mongoCollection;

    public GetDailyReportService(IMongoDbContext<Visitor> mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _mongoCollection = _mongoDbContext.GetCollection();
    }

    #endregion

    public async Task<DailyReportDto> ExecuteAsync()
    {
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(1);

        var totalPageViews = await _mongoCollection.AsQueryable().LongCountAsync();
        var totalVisitors = await _mongoCollection.AsQueryable().GroupBy(x => x.VisitorId).LongCountAsync();
        var totalPageViewsPerVisitor = totalPageViews / totalVisitors;

        var todayPageViews =
            await _mongoCollection.AsQueryable()
                                  .Where(x => x.Time >= startDate && x.Time < endDate)
                                  .LongCountAsync();

        var todayVisitors =
            await _mongoCollection.AsQueryable()
                                  .Where(x => x.Time >= startDate && x.Time < endDate)
                                  .GroupBy(x => x.VisitorId)
                                  .LongCountAsync();

        var todayPageViewsPerVisitor = todayPageViews / todayVisitors;

        var result = new DailyReportDto
        {
            GeneralStats = new()
            {
                TotalVisitors = totalVisitors,
                TotalPageViews = totalPageViews,
                PageViewsPerVisitor = totalPageViewsPerVisitor
            },
            TodayVisitsReport = new()
            {
                Visitors = todayVisitors,
                PageViews = todayPageViews,
                PageViewsPerVisitor = todayPageViewsPerVisitor
            }
        };

        return result;
    }
}