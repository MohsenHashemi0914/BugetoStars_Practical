namespace WebSite.EndPoints.Utilities.Middlewares;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class OnlineVisitorId
{
    private readonly RequestDelegate _next;

    public OnlineVisitorId(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var visitorId = GetCookieValue("VisitorId", httpContext);

        if (string.IsNullOrWhiteSpace(visitorId))
        {
            SetCookie("VisitorId", Guid.NewGuid().ToString(), httpContext);
        }

        return _next.Invoke(httpContext);
    }

    private static string GetCookieValue(string key, HttpContext context)
    {
        return context.Request.Cookies[key];
    }

    private static void SetCookie(string key, string value, HttpContext context)
    {
        context.Response.Cookies.Append(key, value, new()
        {
            Path = "/",
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(25)
        });
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class OnlineVisitorIdExtensions
{
    public static IApplicationBuilder UseOnlineVisitorId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<OnlineVisitorId>();
    }
}