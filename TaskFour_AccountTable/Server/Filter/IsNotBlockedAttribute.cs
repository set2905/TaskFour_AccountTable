using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using TaskFour_AccountTable.Server.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System.Security.Claims;

namespace TaskFour_AccountTable.Server.Filter
{

    public class IsNotBlockedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserManager<User> userManager;

        public IsNotBlockedAttribute(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            ClaimsIdentity? identity = (ClaimsIdentity)context.HttpContext.User.Identity;
            if (identity==null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            string? id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
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
}
