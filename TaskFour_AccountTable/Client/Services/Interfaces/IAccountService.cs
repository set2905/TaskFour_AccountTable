using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Client.Services.Interfaces
{
    public interface IAccountService
    {
        public Task BlockUsers(ICollection<UserViewModel> users);
        public Task<List<UserViewModel>> GetUsers();
    }
}
