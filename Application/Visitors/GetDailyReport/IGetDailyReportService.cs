using Application.Visitors.GetDailyReport.Dtos;

namespace Application.Visitors.GetDailyReport;

public interface IGetDailyReportService
{
    Task<DailyReportDto> ExecuteAsync();
}