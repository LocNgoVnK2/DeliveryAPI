namespace WebApi.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /*
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
    */
    private readonly bool _requireAuthentication;
    private readonly string _role;

    public AuthorizeAttribute(bool requireAuthentication = true, string role = "")
    {
        _requireAuthentication = requireAuthentication;
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (UserVM)context.HttpContext.Items["User"];

        // Check if authentication is required
        if (_requireAuthentication && user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        // Check if role-based authorization is required
        if (_role != "" && (user == null || user.Role != _role))
        {
            // doesn't have the required role
            context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }
    }
}