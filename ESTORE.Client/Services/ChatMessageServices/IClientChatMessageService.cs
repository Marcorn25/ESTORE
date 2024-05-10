using ESTORE.Shared.DTO.ChatMessage;

namespace ESTORE.Client.Services.ChatMessageServices
{
    public interface IClientChatMessageService
    {
        Task<List<ChatMessageDTO>?> GetAllMessages();
    }
}
