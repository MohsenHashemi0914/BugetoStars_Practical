namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record DailyReportDto(GeneralStatsDto GeneralStats,
                                    TodayVisitsReportDto TodayVisitsReport,
                                    Last31DaysVisitsPerDay Last31DaysVisitsPerDay,
                                    Last24HoursVisitsPerHour Last24HoursVisitsPerHour);