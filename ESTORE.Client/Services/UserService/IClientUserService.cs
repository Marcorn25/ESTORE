using ESTORE.Shared.DTO.User;

namespace ESTORE.Client.Services.UserService
{
    public interface IClientUserService
    {
        Task<UserDetailsDTO?> GetUserDetails();
        Task<AddressDTO?> GetUserAddress();
        Task<string?> UpdateUserAddress(AddressDTO address);
        Task<string?> UpdateUserDetails(UserDetailsDTO user);
        Task<string?> ChangeUserPassword(ChangePasswordDTO password);

        //Admin
        Task<List<UserDetailsDTO>?> GetAllUsers();
        Task UpdateUserRole(UserDetailsDTO user);
    }
}
