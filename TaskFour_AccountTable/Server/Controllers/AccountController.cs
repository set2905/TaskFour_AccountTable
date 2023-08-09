using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskFour_AccountTable.Server.Filter;
using TaskFour_AccountTable.Server.Models;
using TaskFour_AccountTable.Shared.Models.Requests;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Server.Controllers
{
    [Authorize]
    [TypeFilter(typeof(IsNotBlockedAttribute))]

    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
        }

        [HttpGet]
        [Route("GetAll")]

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

        [HttpGet]
        [Route("AmIBlocked")]
        public async Task<bool> IsCurrentUserBlocked()
        {
            User? currentuser = await userManager.GetUserAsync(User);
            if (currentuser==null) 
                return true;
            return currentuser.IsBlocked;
        }
        [HttpGet]
        [Route("GetMyId")]
        public async Task<IActionResult> GetCurrentUserId()
        {
            User? currentuser = await userManager.GetUserAsync(User);
            if (currentuser==null) 
                return BadRequest("User not found");
            return new JsonResult(currentuser.Id);
        }
        [HttpPost]
        [Route("SetBlock")]
        public async Task<IActionResult> SetBlock(SetBlockModel setBlockModel)
        {
            List<string> succesfulyChangedIds = new();
            foreach (string userId in setBlockModel.userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null||user.IsBlocked==setBlockModel.blockValue) continue;
                user.IsBlocked = setBlockModel.blockValue;
                if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
                succesfulyChangedIds.Add(userId);
            }
            await LogoutIfBlockedAsync();
            return new JsonResult(succesfulyChangedIds);
        }

        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers(string[] userIds)
        {
            List<string> succesfulyDeletedUsers = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null) continue;
                if (!(await userManager.DeleteAsync(user)).Succeeded) continue;
                succesfulyDeletedUsers.Add(userId);
            }
            return new JsonResult(succesfulyDeletedUsers);
        }

        private async Task LogoutIfBlockedAsync()
        {
            if (await IsCurrentUserBlocked())
                await signInManager.SignOutAsync();

        }
        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel(user.Id, user.Email, user.LastLoginDate, user.RegistrationDate, user.IsBlocked, user.UserName);
        }
    }
}
