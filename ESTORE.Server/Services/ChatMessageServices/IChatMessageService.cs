using ESTORE.Shared.DTO.ChatMessage;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.ChatMessageServices
{
    public interface IChatMessageService
    {
        Task<DataResponse<List<ChatMessageDTO>>> GetAllChatMessage();
        Task<DataResponse<ChatMessageDTO>> AddChatMessage(int userId, string message);

    }
}
