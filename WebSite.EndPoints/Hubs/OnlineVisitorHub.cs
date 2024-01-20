using Application.Visitors.OnlineVisitorService;
using Microsoft.AspNetCore.SignalR;

namespace WebSite.EndPoints.Hubs;

public class OnlineVisitorHub : Hub
{
    #region Constructor

    private readonly IOnlineVisitorService _onlineVisitorService;

    public OnlineVisitorHub(IOnlineVisitorService onlineVisitorService)
    {
        _onlineVisitorService = onlineVisitorService;
    }

    #endregion

    public override async Task OnConnectedAsync()
    {
        var clientId = Context.GetHttpContext().Request.Cookies["VisitorId"];
        await _onlineVisitorService.Add(clientId);
        var onlineVisitorsCount = await _onlineVisitorService.GetOnlineVisitorsCount();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = Context.GetHttpContext().Request.Cookies["VisitorId"];
        await _onlineVisitorService.Remove(clientId);
        var onlineVisitorsCount = await _onlineVisitorService.GetOnlineVisitorsCount();
        await base.OnDisconnectedAsync(exception);
    }
}