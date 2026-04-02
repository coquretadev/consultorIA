using Microsoft.AspNetCore.Mvc;

namespace AiConsulting.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails { Status = 404, Title = "Not Found", Detail = ex.Message };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access");
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails { Status = 401, Title = "Unauthorized", Detail = ex.Message };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument error");
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails { Status = 400, Title = "Bad Request", Detail = ex.Message };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails { Status = 500, Title = "Internal Server Error", Detail = "An unexpected error occurred." };
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
