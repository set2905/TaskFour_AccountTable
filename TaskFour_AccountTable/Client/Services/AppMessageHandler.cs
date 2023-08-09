using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;


namespace TaskFour_AccountTable.Client.Services
{

    public class AppMessageHandler : BaseAddressAuthorizationMessageHandler
    {
        NavigationManager NavigationManager { get; set; }
        public AppMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager) : base(provider, navigationManager)
        {
            NavigationManager=navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("MessageHandler 401");
                NavigationManager.NavigateToLogin("authentication/login");
            }

            return response;

        }
    }
}
