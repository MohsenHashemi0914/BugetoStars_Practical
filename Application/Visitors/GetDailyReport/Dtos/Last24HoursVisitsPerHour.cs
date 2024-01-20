namespace Application.Visitors.GetDailyReport.Dtos;

public sealed record Last24HoursVisitsPerHour(IDictionary<string, long> visitsCountPerHourWithText);