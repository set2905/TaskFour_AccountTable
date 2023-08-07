using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using TaskFour_AccountTable.Server.Models;
using Microsoft.AspNetCore.Http;

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
            //string token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string? userName = context.HttpContext.User.Identity.Name;
            User? user = await userManager.FindByNameAsync(userName);
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
