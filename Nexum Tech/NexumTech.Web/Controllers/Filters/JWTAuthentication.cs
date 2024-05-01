using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NexumTech.Web.Controllers.Filters
{
    public class JwtAuthentication : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (String.IsNullOrEmpty(context.HttpContext.Request.Cookies["jwt"]))
            {
                context.Result = new RedirectToActionResult("Index", "Login", new {requestType = 1});
            }
        }
    }
}
