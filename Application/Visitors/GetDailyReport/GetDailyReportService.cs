using Application.Contexts.Mongo;
using Application.Visitors.GetDailyReport.Dtos;
using Domain.Visitors;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Visitors.GetDailyReport;

public class GetDailyReportService : IGetDailyReportService
{
    #region Constructor

    private readonly IMongoDbContext<Visitor> _context;
    private readonly IMongoCollection<Visitor> _visitors;

    public GetDailyReportService(IMongoDbContext<Visitor> context)
    {
        _context = context;
        _visitors = _context.GetCollection();
    }

    #endregion

    public async Task<DailyReportDto> ExecuteAsync()
    {
        var generalStats = await GetGeneralStatesReport(_visitors);
        var todayVisitsReport = await GetTodayVisitsReport(_visitors);
        var last31DaysVisitsPerDayReport = await GetLast31DaysVisitsPerDayReport(_visitors);
        var last24HoursVisitsPerHourReport = await GetLast24HoursVisitsPerHourReport(_visitors);
        var result = new DailyReportDto(generalStats, todayVisitsReport, last31DaysVisitsPerDayReport, last24HoursVisitsPerHourReport);
        return result;
    }

    #region Utilities

    private static async Task<GeneralStatsDto> GetGeneralStatesReport(IMongoCollection<Visitor> visitors)
    {
        var totalPageViews = await visitors.AsQueryable().LongCountAsync();
        var totalVisitors = await visitors.AsQueryable().GroupBy(x => x.VisitorId).LongCountAsync();
        var totalPageViewsPerVisitor = totalPageViews / totalVisitors;

        return new GeneralStatsDto(totalVisitors, totalPageViews, totalPageViewsPerVisitor);
    }

    private static async Task<TodayVisitsReportDto> GetTodayVisitsReport(IMongoCollection<Visitor> visitors)
    {
        var fromDate = DateTime.UtcNow.Date;
        var toDate = fromDate.AddDays(1);

        var todayPageViews =
            await visitors.AsQueryable()
                          .Where(x => x.Time >= fromDate && x.Time < toDate)
                          .LongCountAsync();

        var todayVisitors =
            await visitors.AsQueryable()
                          .Where(x => x.Time >= fromDate && x.Time < toDate)
                          .GroupBy(x => x.VisitorId)
                          .LongCountAsync();

        var todayPageViewsPerVisitor = todayPageViews / todayVisitors;
        return new TodayVisitsReportDto(todayVisitors, todayPageViews, todayPageViewsPerVisitor);
    }

    private static async Task<Last31DaysVisitsPerDay> GetLast31DaysVisitsPerDayReport(IMongoCollection<Visitor> visitors)
    {
        var currentDate = DateTime.UtcNow.Date;
        var fromDate = currentDate.AddDays(-31);

        var visitsInPeriod = await visitors.AsQueryable()
                                           .Where(x => x.Time.Date >= fromDate && x.Time.Date <= currentDate)
                                           .Select(x => new { x.Time })
                                           .ToListAsync();

        var dictionary = new Dictionary<string, long>();

        for (int i = 0; i < 31; i++)
        {
            var daysBefore = currentDate.AddDays(i * -1);
            var daysBeforeVisitsCount = visitsInPeriod.LongCount(x => x.Time.Date == daysBefore.Date);
            dictionary.Add(i.ToString(), daysBeforeVisitsCount);
        }

        return new Last31DaysVisitsPerDay(dictionary);
    }

    private static async Task<Last24HoursVisitsPerHour> GetLast24HoursVisitsPerHourReport(IMongoCollection<Visitor> visitors)
    {
        var currentDate = DateTime.UtcNow.Date;
        var toDate = currentDate.AddDays(1);

        var visitorsInPeriod = await visitors.AsQueryable()
                                             .Where(x => x.Time >= currentDate && x.Time < toDate)
                                             .Select(x => new { x.Time })
                                             .ToListAsync();

        var dictionary = new Dictionary<string, long>();

        for (int i = 0; i < 24; i++)
        {
            var hourVisitsCount = visitorsInPeriod.LongCount(x => x.Time.Hour == i);
            dictionary.Add(i.ToString(), hourVisitsCount);
        }

        return new Last24HoursVisitsPerHour(dictionary);
    }

    #endregion
}