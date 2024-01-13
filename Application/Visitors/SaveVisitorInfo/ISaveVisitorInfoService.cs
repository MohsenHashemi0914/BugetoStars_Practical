using Application.Visitors.SaveVisitorInfo.Dtos;
using Domain.Visitors;

namespace Application.Visitors.SaveVisitorInfo;

public interface ISaveVisitorInfoService
{
    Task ExecuteAsync(SaveVisitorInfoDto command);
}