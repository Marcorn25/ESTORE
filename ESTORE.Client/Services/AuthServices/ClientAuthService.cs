using ESTORE.Shared.DTO.User;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using ESTORE.Client.States.User;
using Microsoft.AspNetCore.Components.Authorization;

namespace ESTORE.Client.Services.AuthServices
{
    public class ClientAuthService : IClientAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;
        private readonly NavigationManager navigationManager;

        public ClientAuthService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
            this.navigationManager = navigationManager;
        }

        public Token Token { get; set; } = new Token();
        public User User { get; set; } = new User();

        private async Task<string> SetToken(HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                Token.Value = await message.Content.ReadAsStringAsync();
                return "success";
            }
            else
            {
                return await message.Content.ReadAsStringAsync();
            }
        }

        public async Task<string?> LoginAccount(LoginDTO user)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", user);

            if (response.IsSuccessStatusCode)
            {
 
                var data = await SetToken(response);
                navigationManager.NavigateTo("/");
                return null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        public async Task<string?> RegisterAccount(RegisterDTO user)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", user);

            if (response.IsSuccessStatusCode)
            {
                var data = await SetToken(response);
                navigationManager.NavigateTo("/");
                return null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }

        public async Task LogoutAccount()
        {
            var response = await httpClient.GetStringAsync("api/auth/logout");
            if(response != null)
            {
                Token.Value = "";
                User = new User();
                navigationManager.NavigateTo("/");
            }
        }
    }
}
