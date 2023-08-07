using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskFour_AccountTable.Server.Models;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Server.Controllers
{

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
        public IEnumerable<UserViewModel> Get()
        {
            IQueryable<User> users = userManager.Users;
            List<UserViewModel> usersViewModels = new List<UserViewModel>();
            foreach (User u in users)
            {
                usersViewModels.Add(new UserViewModel(u.Id, u.Email, u.LastLoginDate, u.RegistrationDate, u.IsBlocked, u.UserName));
            }
            return usersViewModels;
        }
    }
}
