using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<UserDisplayModel> Get()
        {
            return new List<UserDisplayModel> { new() { Name="1" } };
        }
    }
}
