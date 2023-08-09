using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskFour_AccountTable.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace TaskFour_AccountTable.Server.Filter
{

    public class DenyBlockedAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> userManager;

        public DenyBlockedAttribute(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var attributes = descriptor.MethodInfo.CustomAttributes;
            if (attributes.Any(a => a.AttributeType == typeof(AllowBlockedAttribute))) return;

            ClaimsIdentity? identity = (ClaimsIdentity?)context.HttpContext.User.Identity;
            if (identity==null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            string? id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value)
                   .SingleOrDefault();
            if (id==null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            User? user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (user.IsBlocked)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

    }
    public class AllowBlockedAttribute : Attribute
    {
    }
}
