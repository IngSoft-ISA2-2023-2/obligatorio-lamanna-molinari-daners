using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PharmaGo.IBusinessLogic;

namespace PharmaGo.WebApi.Filters
{
    public class AuthorizationFilter : Attribute, IActionFilter
    {

        private readonly string[] _roles;

        public AuthorizationFilter(string[] roles)
        {
            _roles = roles;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var _loginManager = GetSessions(context);
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (String.IsNullOrEmpty(token) || !_loginManager.IsTokenValid(token))
            {
                context.Result = new JsonResult(new { Message = "Invalid authorization token" })
                { StatusCode = 401 };
            } else if (!_loginManager.IsRoleValid(_roles, token))
            {
                context.Result = new JsonResult(new { Message = "Forbidden role" })
                { StatusCode = 403 };
            }
        }

        private static ILoginManager GetSessions(ActionExecutingContext Context)
        {
            return (ILoginManager)Context.HttpContext.RequestServices.GetService(typeof(ILoginManager));
        }

    }
}
