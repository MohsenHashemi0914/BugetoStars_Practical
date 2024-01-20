namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record Last31DaysVisitsPerDay(IDictionary<string, long> visitsCountPerDayWithText);