using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Client.Services.Interfaces
{
    public interface IAccountService
    {
        public Task SetBlock(IEnumerable<UserViewModel> users, bool blockValue = true);
        public Task<IEnumerable<string>> DeleteUsers(IEnumerable<UserViewModel> users);
        public Task<List<UserViewModel>> GetAllUsers();
        public Task<bool> IsCurrentUserBlocked();
        public Task<string> GetCurrentUserId();
    }
}
