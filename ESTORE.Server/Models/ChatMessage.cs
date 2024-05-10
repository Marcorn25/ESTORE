namespace ESTORE.Server.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Content { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
