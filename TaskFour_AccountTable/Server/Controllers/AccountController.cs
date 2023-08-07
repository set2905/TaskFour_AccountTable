using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskFour_AccountTable.Server.Models;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Server.Controllers
{

    [Authorize(Policy = "IsBlockedPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;


        public AccountController(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> Get()
        {
            List<User> users = await userManager.Users.ToListAsync();
            List<UserViewModel> usersViewModels = new List<UserViewModel>();
            foreach (User user in users)
            {
                usersViewModels.Add(GetUserViewModel(user));
            }
            return usersViewModels;
        }
        public async Task<IEnumerable<UserViewModel>> BlockUsers(string[] userIds)
        {
            List<UserViewModel> succesfulyBlockedUsers = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null) continue;
                if (user.IsBlocked) continue;
                user.IsBlocked = true;
                await userManager.UpdateAsync(user);
                succesfulyBlockedUsers.Add(GetUserViewModel(user));
               // await userManager.RemoveAuthenticationTokenAsync(user,);
                //userManager.Log

            }
            return succesfulyBlockedUsers;
        }

        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel(user.Id, user.Email, user.LastLoginDate, user.RegistrationDate, user.IsBlocked, user.UserName);
        }
    }
}
