using ESTORE.Shared.DTO.User;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.UserServices
{
    public interface IUserService
    {
        Task<DataResponse<UserDetailsDTO>> GetAuthenticatedUserDetails();
        Task<GeneralResponse> UpdateUserAddress(AddressDTO address);
        Task<GeneralResponse> UpdateUserDetails(UserDetailsDTO user, int Id);
        Task<DataResponse<AddressDTO>> GetUserAddress();
        //Admin
        Task<DataResponse<List<UserDetailsDTO>>> GetAllUsers();
        Task<GeneralResponse> UpdateUserRole(UserDetailsDTO user);
    }
}
