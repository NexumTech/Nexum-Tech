using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace NexumTech.Infra.API.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Culture", out var cultureHeaderValue))
            {
                var culture = new CultureInfo(cultureHeaderValue.ToString());
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            await _next(context);
        }
    }
}
