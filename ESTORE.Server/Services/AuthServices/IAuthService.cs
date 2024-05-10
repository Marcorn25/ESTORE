using ESTORE.Shared.DTO.User;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResponse> CreateAccount(RegisterDTO user);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
        Task<GeneralResponse> UpdatePassword(ChangePasswordDTO passwordDTO);

    }
}
