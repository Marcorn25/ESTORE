using ESTORE.Shared.Enum;

namespace ESTORE.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public AccountRoles Role { get; set; } = AccountRoles.USER;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarURL { get; set; } = string.Empty;
        public Address? HomeAddress { get; set; }

        public List<CartItem> CartItems { get; set; }
        public List<Order>? Orders { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }


    }
}
