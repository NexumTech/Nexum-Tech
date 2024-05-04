using Microsoft.AspNetCore.Http;
using System.Globalization;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var cultureCookie = context.Request.Cookies["Culture"];
        var defaultCulture = "en-US"; 

        if (!string.IsNullOrEmpty(cultureCookie))
        {
            defaultCulture = cultureCookie;
        }

        var cultureInfo = new CultureInfo(defaultCulture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}