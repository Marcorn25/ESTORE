using ESTORE.Shared.DTO.ChatMessage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.ChatMessageServices
{
    public class ClientChatMessageService : IClientChatMessageService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;
        private readonly NavigationManager navigationManager;

        public ClientChatMessageService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
            this.navigationManager = navigationManager;
        }
        public async Task<List<ChatMessageDTO>?> GetAllMessages()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<ChatMessageDTO>>("api/chats");
                if (response != null)
                {
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
                return null;
            }
        }
    }
}