using System.Net.Http.Json;
using TaskFour_AccountTable.Client.Pages;
using TaskFour_AccountTable.Client.Services.Interfaces;
using TaskFour_AccountTable.Shared.UserDisplayModel;
using static System.Net.WebRequestMethods;

namespace TaskFour_AccountTable.Client.Services
{
    public class AccountService : IAccountService
    {
        private HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task BlockUsers(ICollection<UserViewModel> users)
        {
            var postBody = new { userIds = users.Select(x => x.Id).ToArray() };
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("Account/SetBlock", postBody);
            //HttpResponseMessage response = await Http.PostAsJsonAsync<string[]>("Account/setblock", selectedItems.Select(x => x.Id.ToString()).ToArray());
            IEnumerable<string>? result = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            foreach (UserViewModel user in users.Where(x => result.Contains(x.Id))) user.IsBlocked = true;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await httpClient.GetFromJsonAsync<List<UserViewModel>>("Account")??new();
            return users;

        }
    }
}
