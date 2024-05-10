using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.ChatMessageServices;
using ESTORE.Server.Services.ProductServices;
using ESTORE.Shared.DTO.ChatMessage;
using ESTORE.Shared.Enum;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ESTORE.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext context;
        private readonly IChatMessageService chatMessageService;
        public ChatHub(DataContext context, IChatMessageService chatMessageService)
        {
            this.context = context;
            this.chatMessageService = chatMessageService;
        }

        public async Task SendMessage(int userId, string message)
        {
           var response = await chatMessageService.AddChatMessage(userId, message);

            if(response.ResponseCode == HttpStatusCode.OK)
                await Clients.All.SendAsync("ReceiveMessage", response.Data);
        }
    }
}
