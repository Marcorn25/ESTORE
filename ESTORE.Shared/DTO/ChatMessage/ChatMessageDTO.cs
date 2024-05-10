using ESTORE.Shared.DTO.User;

namespace ESTORE.Shared.DTO.ChatMessage
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Content { get; set; }
        public UserChatMessageDTO? User { get; set; }
    }
}
