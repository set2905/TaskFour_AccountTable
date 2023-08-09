using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Client.Services.Interfaces
{
    public interface IAccountService
    {
        public Task SetBlock(IEnumerable<UserViewModel> users, bool blockValue = true);
        public Task<List<UserViewModel>> GetAllUsers();
    }
}
