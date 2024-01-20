namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record GeneralStatsDto(long TotalVisitors, long TotalPageViews, long PageViewsPerVisitor);