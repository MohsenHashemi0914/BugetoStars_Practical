namespace Application.Visitors.SaveVisitorInfo.Dtos;

public sealed record VisitorVersionDto
{
    public string Family { get; init; }
    public string Version { get; init; }
}