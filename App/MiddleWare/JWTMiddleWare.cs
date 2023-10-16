using Simbir.GO.App.Services;
using System.Net;

namespace Simbir.GO.App.MiddleWare
{
    public class JWTMiddleWare
    {
        private readonly RequestDelegate _next;

        public JWTMiddleWare( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, JWTService _tokenService)
        {
            string? token = context.Session.GetString("token");

            if (token!=null && !await _tokenService.TokenIsValid(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(context);
        }
    }
}
