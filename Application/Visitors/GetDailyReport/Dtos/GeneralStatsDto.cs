namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record GeneralStatsDto
{
    public long TotalVisitors { get; init; }
    public long TotalPageViews { get; init; }
    public long PageViewsPerVisitor { get; init; }
}