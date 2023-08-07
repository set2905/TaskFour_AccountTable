using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFour_AccountTable.Shared.UserDisplayModel
{
    public class UserViewModel
    {
        public UserViewModel(string id, string? email, DateTime lastLogin, DateTime registrationDate, bool isBlocked, string? name)
        {
            Id=id;
            Email=email;
            LastLogin=lastLogin;
            RegistrationDate=registrationDate;
            IsBlocked=isBlocked;
            Name=name;
        }

        public string Id { get; set; } 
        public string? Email { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }  
        public bool IsBlocked { get; set; }
        public string? Name { get; set; }
    }
}
