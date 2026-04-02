using System.Security.Cryptography;
using System.Text;
using AiConsulting.Application.Services;

namespace AiConsulting.Api.Middleware;

public class AnalyticsTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public AnalyticsTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Get &&
            context.Request.Path.StartsWithSegments("/api/public"))
        {
            try
            {
                var analyticsService = context.RequestServices.GetRequiredService<IAnalyticsService>();

                var page = context.Request.Path.Value ?? "/";
                var referrer = context.Request.Headers["Referer"].FirstOrDefault();
                var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
                var ipHash = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(
                        context.Connection.RemoteIpAddress?.ToString() ?? "unknown")));

                await analyticsService.RecordVisitAsync(page, referrer, userAgent, ipHash);
            }
            catch
            {
                // Don't block the request on analytics failure
            }
        }

        await _next(context);
    }
}
