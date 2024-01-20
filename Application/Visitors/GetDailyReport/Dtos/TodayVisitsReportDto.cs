namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record TodayVisitsReportDto(long Visitors, long PageViews, long PageViewsPerVisitor);