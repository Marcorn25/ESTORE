namespace ESTORE.Server.Services.BlobStorageServices
{
    public interface IBlobStorageService
    {
        Task<string> UploadProductImage(IFormFile request);
        Task<bool> DeleteFileToBlobAsync(string strFileName);
        Task<string> EditUploadedProductImage(IFormFile request, int Id);

        //User
        Task<string> UploadUserAvatar(IFormFile request);
        Task<string> EditUploadedUserImage(IFormFile request, int Id);



    }
}
