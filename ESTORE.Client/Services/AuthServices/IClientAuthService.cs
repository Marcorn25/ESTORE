using ESTORE.Client.States.User;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Client.Services.AuthServices
{
    public interface IClientAuthService
    {
        Token Token { get; set; }
        User User { get; set; }
        Task<string?> LoginAccount(LoginDTO user);
        Task<string?> RegisterAccount(RegisterDTO user);
        Task LogoutAccount();

    }
}
