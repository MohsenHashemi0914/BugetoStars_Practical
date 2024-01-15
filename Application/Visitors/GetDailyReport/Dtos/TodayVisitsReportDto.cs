namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record TodayVisitsReportDto
{
    public long Visitors { get; init; }
    public long PageViews { get; init; }
    public long PageViewsPerVisitor { get; init; }
}