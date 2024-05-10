using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Shared.DTO.ChatMessage;
using Microsoft.EntityFrameworkCore;
using static ESTORE.Server.Services.ResponseServices.Response;
using System.Net;
using ESTORE.Server.Services.DataServices;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Server.Services.ChatMessageServices
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IDataService dataService;

        public ChatMessageService(DataContext context, IHttpContextAccessor contextAccessor, IDataService dataService)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
            this.dataService = dataService;
        }

        public async Task<DataResponse<ChatMessageDTO>> AddChatMessage(int userId, string message)
        {
            try
            {
                var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return new DataResponse<ChatMessageDTO>(null!, "No User Found", HttpStatusCode.BadRequest);

                var newMessage = new ChatMessage { User = user, Content = message, TimeStamp = DateTime.Now };
                context.ChatMessages.Add(newMessage);
                await context.SaveChangesAsync();

                return new DataResponse<ChatMessageDTO>(dataService.CastToChatMessageDTO(newMessage), "Message Sent");
            }
            catch
            {
                return new DataResponse<ChatMessageDTO>(null!, "Error Occured", HttpStatusCode.BadRequest);
            }

        }

        public async Task<DataResponse<List<ChatMessageDTO>>> GetAllChatMessage()
        {
            var response = await context.ChatMessages
                .Include(cm => cm.User)
                .ThenInclude(user => user.HomeAddress)
                .ToListAsync();

            var chatMessageDTOs = response.Select(cm => dataService.CastToChatMessageDTO(cm)).ToList();

            if (response != null)
                return new DataResponse<List<ChatMessageDTO>>(chatMessageDTOs, "Messages fetched");
            return new DataResponse<List<ChatMessageDTO>>(null!, "Error Occured... Please Try Again");

        }

    }
}
