using ESTORE.Shared.DTO.Product;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace ESTORE.Client.Services.FileUploadServices
{
    public class ClientFileUploadService : IClientFileUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public ClientFileUploadService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<MultipartFormDataContent?> ImageTOMultipartFormDataContent(InputFileChangeEventArgs e)
        {
            var imageFile = e.File;
            if (imageFile == null)
                return null;

            var resizedFile = await imageFile.RequestImageFileAsync("image/png", 300, 500);

            using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
            {
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFile.Name);
                return content;
            }
        }

        public async Task<string?> UploadFile(IBrowserFile payload)
        {
            long max_file_size = 1024 * 1024 * 5;
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(payload.OpenReadStream(max_file_size));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(payload.ContentType);

            content.Add(
                content: fileContent,
                name: "\"files\"",
            fileName: payload.Name);

            var response = await _httpClient.PostAsync("api/BlobStorage/upload-product-image", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return string.Empty;
        }

        public async Task<string?> EditUploadedFile(IBrowserFile payload, int productId)
        {
            long max_file_size = 1024 * 1024 * 5;
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(payload.OpenReadStream(max_file_size));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(payload.ContentType);

            content.Add(
                content: fileContent,
                name: "\"files\"",
            fileName: payload.Name);

            var response = await _httpClient.PostAsync($"api/BlobStorage/edit-uploaded-image/{productId}", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return string.Empty;
        }

        //User
        public async Task<string?> UploadUserImage(IBrowserFile payload)
        {
            long max_file_size = 1024 * 1024 * 5;
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(payload.OpenReadStream(max_file_size));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(payload.ContentType);

            content.Add(
                content: fileContent,
                name: "\"files\"",
            fileName: payload.Name);

            var response = await _httpClient.PostAsync("api/BlobStorage/upload-user-image", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return string.Empty;
        }
        public async Task<string?> EditUserImage(IBrowserFile payload, int userId)
        {
            long max_file_size = 1024 * 1024 * 5;
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(payload.OpenReadStream(max_file_size));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(payload.ContentType);

            content.Add(
                content: fileContent,
                name: "\"files\"",
            fileName: payload.Name);

            var response = await _httpClient.PostAsync($"api/BlobStorage/edit-user-image/{userId}", content);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return string.Empty;
        }

    }
}
