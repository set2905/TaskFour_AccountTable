using Microsoft.AspNetCore.Identity;

namespace TaskFour_AccountTable.Server.Models
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
    }
}