
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
       
        _logger.LogInformation("Headers:");
        foreach (var header in context.Request.Headers)
        {
            _logger.LogInformation($"{header.Key}: {header.Value}");
        }

       
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null)
        {
            _logger.LogInformation($"authorization Header: {authHeader}");

            
            var token = authHeader.StartsWith("Bearer ")
                ? authHeader.Substring(7)
                : authHeader;

            _logger.LogInformation($"token: {token}");
        }
        else
        {
            _logger.LogWarning("no Auth header present");
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