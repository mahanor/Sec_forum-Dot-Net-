using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Sec_Forum
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetString("User_uid");

            if (userId == null)
            {
                // redirect user to login page
                context.Result = new RedirectToActionResult("Login", "UserMasters", null);
            }
        }
    }
}
