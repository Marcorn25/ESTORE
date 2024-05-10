using ESTORE.Shared.Enum;

namespace ESTORE.Shared.DTO.User
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AccountRoles? Role { get; set; } = AccountRoles.USER;
        public string AvatarURL { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public AddressDTO? HomeAddress { get; set; }
    }
}
