
public class JwtDebugMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtDebugMiddleware> _logger;

    public JwtDebugMiddleware(RequestDelegate next, ILogger<JwtDebugMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log all headers
        _logger.LogInformation("Request Headers:");
        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation($"{header.Key}: {header.Value}");
        }

        // Specifically log Authorization header
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null)
        {
            _logger.LogInformation($"Authorization Header: {authHeader}");

            // Parse and validate JWT format
            var token = authHeader.StartsWith("Bearer ")
                ? authHeader.Substring(7)
                : authHeader;

            _logger.LogInformation($"Extracted Token: {token}");
        }
        else
        {
            _logger.LogWarning("No Authorization header present");
        }

        await _next(context);
    }
}


public static class JwtDebugMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtDebugMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtDebugMiddleware>();
    }
}