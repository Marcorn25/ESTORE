using Microsoft.AspNetCore.Components.Forms;

namespace ESTORE.Client.Services.FileUploadServices
{
    public interface IClientFileUploadService
    {
        Task<MultipartFormDataContent?> ImageTOMultipartFormDataContent(InputFileChangeEventArgs e);
        Task<string?> UploadFile(IBrowserFile contentFile);
        Task<string?> EditUploadedFile(IBrowserFile payload, int productId);

        //User
        Task<string?> UploadUserImage(IBrowserFile payload);
        Task<string?> EditUserImage(IBrowserFile payload, int userId);

    }
}
