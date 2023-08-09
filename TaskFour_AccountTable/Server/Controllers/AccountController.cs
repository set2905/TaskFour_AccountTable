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
    [TypeFilter(typeof(DenyBlockedAttribute))]

    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public AccountController(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        /// <returns>
        /// All existing users from the database
        /// </returns>
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

        /// <summary>
        /// Sets block status on users with specified Ids
        /// </summary>
        /// <param name="setBlockModel.userIds"></param>
        /// <returns>
        /// Array of user ids that has been successfully changed
        /// </returns>
        [HttpPost]
        [Route("SetBlock")]
        public async Task<IActionResult> SetBlock(SetBlockModel setBlockModel)
        {
            if(setBlockModel==null)
                return BadRequest("Passed model is null");
            List<string> succesfulyChangedIds = new();
            foreach (string userId in setBlockModel.userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null||user.IsBlocked==setBlockModel.blockValue) continue;
                user.IsBlocked = setBlockModel.blockValue;
                if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
                succesfulyChangedIds.Add(userId);
            }
            return new JsonResult(succesfulyChangedIds);
        }
        /// <summary>
        /// Deletes users with <paramref name="userIds"/> from the database
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns>
        /// Array of user ids that has been successfully changed
        /// </returns>
        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers(string[] userIds)
        {
            if (userIds==null)
                return BadRequest("Passed userIds is null");
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

        private UserViewModel GetUserViewModel(User user)
        {
            return new UserViewModel(user.Id, user.Email, user.LastLoginDate, user.RegistrationDate, user.IsBlocked, user.UserName);
        }
    }
}
