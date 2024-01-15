namespace Application.Visitors.SaveVisitorInfo.Dtos;

public sealed record SaveVisitorInfoDto
{
    public string IP { get; init; }
    public string Method { get; init; }
    public string Protocol { get; init; }
    public string VisitorId { get; init; }
    public string CurrentLink { get; init; }
    public string ReferrerLink { get; init; }
    public string PhysicalPath { get; init; }

    public DeviceDto Device { get; init; }
    public VisitorVersionDto Browser { get; init; }
    public VisitorVersionDto OperationSystem { get; init; }
}