using MudBlazor;
using System.Net.Http.Json;
using TaskFour_AccountTable.Client.Services.Interfaces;
using TaskFour_AccountTable.Shared.Models.Requests;
using TaskFour_AccountTable.Shared.UserDisplayModel;

namespace TaskFour_AccountTable.Client.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task SetBlock(IEnumerable<UserViewModel> users, bool blockValue = true)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("Account/SetBlock", new SetBlockModel(users.Select(x => x.Id).ToArray(), blockValue));
            IEnumerable<string>? result = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            foreach (UserViewModel user in users.Where(x => result.Contains(x.Id))) user.IsBlocked = blockValue;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var users = await httpClient.GetFromJsonAsync<List<UserViewModel>>("Account/GetAll")??new();
            return users;

        }
    }



}
