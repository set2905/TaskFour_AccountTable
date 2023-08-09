using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;

namespace TaskFour_AccountTable.Client.Services
{

    public class AppMessageHandler : BaseAddressAuthorizationMessageHandler
    {
        private readonly NavigationManager navigationManager;
        private readonly ISnackbar snackbar;

        public AppMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, ISnackbar snackbar) : base(provider, navigationManager)
        {
            this.navigationManager=navigationManager;
            this.snackbar=snackbar;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("MessageHandler 401");
                snackbar.Add("Unauthorized to perform action. You are being logged out.", Severity.Error);
                navigationManager.NavigateToLogout("authentication/logout");
            }

            return response;

        }
    }
}
