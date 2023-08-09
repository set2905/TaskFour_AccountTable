using MudBlazor;
using System.Net.Http.Json;
using TaskFour_AccountTable.Shared.Models.Requests;
using static System.Net.WebRequestMethods;

namespace TaskFour_AccountTable.Client.Services
{
    public abstract class ClientAPIBase
    {
        private readonly HttpClient httpClient;

        public ClientAPIBase(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        protected async Task<TReturn?> GetAsync<TReturn>(string relativeUri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(relativeUri);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TReturn>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
        protected async Task<TReturn?> PostAsync<TReturn, TRequest>(string relativeUri, TRequest requestParam)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(relativeUri, requestParam);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());              
            }


            TReturn? result = await response.Content.ReadFromJsonAsync<TReturn>();
            return result;

        }
    }
}
