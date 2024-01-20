namespace Application.Visitors.OnlineVisitorService;

public interface IOnlineVisitorService
{
    Task Add(string clientId);
    Task Remove(string clientId);
    Task<long> GetOnlineVisitorsCount();
}