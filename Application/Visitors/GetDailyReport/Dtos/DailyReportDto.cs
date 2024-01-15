namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record DailyReportDto
{
    public GeneralStatsDto GeneralStats { get; init; }
    public TodayVisitsReportDto TodayVisitsReport { get; init; }
}