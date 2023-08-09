using MudBlazor;
using TaskFour_AccountTable.Client.Services.Interfaces;
using TaskFour_AccountTable.Shared.Models.Requests;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Client.Services
{
    public class AccountClientService : ClientAPIBase, IAccountService
    {
        private readonly ISnackbar snackbar;

        public AccountClientService(HttpClient httpClient, ISnackbar snackbar) : base(httpClient)
        {
            this.snackbar = snackbar;
        }

        public async Task SetBlock(IEnumerable<UserViewModel> users, bool blockValue = true)
        {
            IEnumerable<string>? result;
            try
            {
                result = await PostAsync<IEnumerable<string>, SetBlockModel>(
                    "Account/SetBlock",
                    new SetBlockModel(users.Select(x => x.Id).ToArray(), blockValue));
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return;
            }

            if (result==null) return;

            foreach (UserViewModel user in users.Where(x => result.Contains(x.Id)))
            {
                user.IsBlocked = blockValue;
            }
        }
        /// <summary>
        /// Sends POST request to delete <paramref name="users"/>
        /// </summary>
        /// <param name="users"></param>
        /// <returns>
        /// Users that were successfuly deleted
        /// </returns>
        public async Task<IEnumerable<string>?> DeleteUsers(IEnumerable<UserViewModel> users)
        {
            try
            {
                return await PostAsync<IEnumerable<string>, string[]>("Account/DeleteUsers", users.Select(x => x.Id).ToArray())??new List<string>();
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;

            }
        }

        public async Task<List<UserViewModel>?> GetAllUsers()
        {
            try
            {
                return await GetAsync<List<UserViewModel>>("Account/GetAll")??new();
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }

        }
    }



}
