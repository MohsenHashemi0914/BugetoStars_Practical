namespace Application.Visitors.SaveVisitorInfo.Dtos;

public sealed record DeviceDto
{
    public string Brand { get; init; }
    public string Model { get; init; }
    public string Family { get; init; }
    public bool IsSpider { get; init; }
}