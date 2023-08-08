using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskFour_AccountTable.Server.Filter;
using TaskFour_AccountTable.Server.Models;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Server.Controllers
{
    [TypeFilter(typeof(IsNotBlockedAttribute))]
    [Authorize]
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
        //[HttpPost("{userIds, valueToSet}")]
        [HttpPost]

        public async Task<IActionResult> SetBlock(string[] userIds)
        {
            List<string> succesfulyChangedIds = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null||user.IsBlocked==true) continue;
                user.IsBlocked = true;
                if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
                succesfulyChangedIds.Add(userId);
            }
            return new JsonResult(succesfulyChangedIds);
        }

        //[HttpPost]

        //public async Task<IActionResult> DeleteUsers(string[] userIds)
        //{
        //    List<string> succesfulyDeletedUsers = new();
        //    foreach (string userId in userIds)
        //    {
        //        User? user = await userManager.FindByIdAsync(userId);
        //        if (user == null||user.IsBlocked) continue;
        //        user.IsBlocked = true;
        //        if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
        //        succesfulyDeletedUsers.Add(userId);
        //    }
        //    return new JsonResult(succesfulyDeletedUsers);
        //}

        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel(user.Id, user.Email, user.LastLoginDate, user.RegistrationDate, user.IsBlocked, user.UserName);
        }
    }
}
