
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ESTORE.Server.Services.BlobStorageServices
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient _client;
        private readonly DataContext _context;

        public BlobStorageService(BlobServiceClient blobServiceClient, DataContext context)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _client = _blobServiceClient.GetBlobContainerClient("freshkoimg");
        }

        public async Task<bool> DeleteFileToBlobAsync(string imageUrl)
        {
            var fileName = new Uri(imageUrl).Segments.LastOrDefault();

            if (string.IsNullOrEmpty(fileName)) return false;

            string blob_name = $"/product-image/mar/product/" + fileName;

            var blobClient = _client.GetBlobClient(blob_name);

            bool result = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            if (!result) return false;

            return true;
        }

        public async Task<string> UploadProductImage(IFormFile request)
        {
            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("yyyy-MM-dd-HH-mm-ss");

            string extension = Path.GetExtension(request.FileName);
            string filename_for_storage = Guid.NewGuid().ToString() + "-" + formattedDate + extension;

            var response = await _client.UploadBlobAsync("/product-image/mar/product/" + filename_for_storage, request.OpenReadStream());

            if (response.GetRawResponse().Status == 201)
            {
                return $"https://sbcstorage.blob.core.windows.net/freshkoimg/product-image/mar/product/{filename_for_storage}";
            }
            return string.Empty;
        }

        public async Task<string> EditUploadedProductImage(IFormFile request, int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
                return string.Empty;

            var deleteFirstBlob = await DeleteFileToBlobAsync(product.ImageUrl);
            if (!deleteFirstBlob)
                return string.Empty;

            var uploadNewImage = await UploadProductImage(request);
            if (!string.IsNullOrEmpty(uploadNewImage))
                return uploadNewImage;
            return string.Empty;
        }

        //User
        public async Task<string> UploadUserAvatar(IFormFile request)
        {
            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("yyyy-MM-dd-HH-mm-ss");

            string extension = Path.GetExtension(request.FileName);
            string filename_for_storage = Guid.NewGuid().ToString() + "-" + formattedDate + extension;

            var response = await _client.UploadBlobAsync("/product-image/mar/user/" + filename_for_storage, request.OpenReadStream());

            if (response.GetRawResponse().Status == 201)
            {
                return $"https://sbcstorage.blob.core.windows.net/freshkoimg/product-image/mar/user/{filename_for_storage}";
            }
            return string.Empty;
        }
        public async Task<bool> DeleteUserBlobAsync(string imageUrl)
        {
            var fileName = new Uri(imageUrl).Segments.LastOrDefault();

            if (string.IsNullOrEmpty(fileName)) return false;

            string blob_name = $"/product-image/mar/user/" + fileName;

            var blobClient = _client.GetBlobClient(blob_name);

            bool result = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            if (!result) return false;

            return true;
        }
        public async Task<string> EditUploadedUserImage(IFormFile request, int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user == null)
                return string.Empty;

            var deleteFirstBlob = await DeleteUserBlobAsync(user.AvatarURL);
            if (!deleteFirstBlob)
                return string.Empty;

            var uploadNewImage = await UploadUserAvatar(request);
            if (!string.IsNullOrEmpty(uploadNewImage))
                return uploadNewImage;
            return string.Empty;
        }
    }
}
