using Application.Visitors.SaveVisitorInfo;
using Application.Visitors.SaveVisitorInfo.Dtos;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using UAParser;

namespace WebSite.EndPoints.Utilities.Filters;

public class SaveVisitorInfoActionFilter : IActionFilter
{
    #region Constructor

    private readonly ISaveVisitorInfoService _saveVisitorInfoService;

    public SaveVisitorInfoActionFilter(ISaveVisitorInfoService saveVisitorInfoService)
    {
        _saveVisitorInfoService = saveVisitorInfoService;
    }

    #endregion

    public void OnActionExecuted(ActionExecutedContext context) { }

    public async void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var ip = request.HttpContext.Connection.RemoteIpAddress.ToString();
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var actionName = actionDescriptor.ActionName;
        var controllerName = actionDescriptor.ControllerName;
        var userAgent = request.Headers["User-Agent"];
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);
        var referrerLink = request.Headers["Referrer"].ToString();
        var currentLink = request.Path.ToString();

        var visitor = new SaveVisitorInfoDto
        {
            IP = ip,
            Method = request.Method,
            CurrentLink = currentLink,
            ReferrerLink = referrerLink,
            Protocol = request.Protocol,
            PhysicalPath = $"{controllerName}/{actionName}",
            Device = new()
            {
                Brand = clientInfo.Device.Brand,
                Model = clientInfo.Device.Model,
                Family = clientInfo.Device.Family,
                IsSpider = clientInfo.Device.IsSpider
            },
            Browser = new()
            {
                Family = clientInfo.UA.Family,
                Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}"
            },
            OperationSystem = new()
            {
                Family = clientInfo.OS.Family,
                Version = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}"
            }
        };

        await _saveVisitorInfoService.ExecuteAsync(visitor);
    }
}