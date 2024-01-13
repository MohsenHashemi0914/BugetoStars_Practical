using Application.Visitors.SaveVisitorInfo.Dtos;
using Domain.Visitors;

namespace Application.Visitors.SaveVisitorInfo;

public interface ISaveVisitorInfoService
{
    void Execute(SaveVisitorInfoDto command);
}