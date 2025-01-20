
using System.Security.Claims;
using ArticlesServer.Services;

namespace ArtcilesServer.Middlewares
{
    public class JWTMiddleware : IMiddleware
    {
        private readonly AuthService _authService;
        public JWTMiddleware(AuthService authService)
        {

            _authService = authService;

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                try
                {

                    var claimsPrincipal = _authService.ValidateToken(token);
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
              
                    context.Items["userId"] = userId;




                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }


            }

            await next(context);
        }
    }

}