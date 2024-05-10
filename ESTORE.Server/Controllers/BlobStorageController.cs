using Azure.Storage.Blobs;
using ESTORE.Server.Models;
using ESTORE.Server.Services.BlobStorageServices;
using ESTORE.Server.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESTORE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService blobStorageService;

        public BlobStorageController(IBlobStorageService blobStorageService)
        {
            this.blobStorageService = blobStorageService;
        }

        [HttpPost("upload-product-image")]
        public async Task<IActionResult> UploadProductImage(IFormFile files)
        {
            string response = await blobStorageService.UploadProductImage(files);
            return Ok(response);
        }

        [HttpPost("edit-uploaded-image/{Id}")]
        public async Task<IActionResult> EditUploadedProductImage(IFormFile files, int Id)
        {
            string response = await blobStorageService.EditUploadedProductImage(files, Id);
            return Ok(response);
        }

        //User
        [HttpPost("upload-user-image")]
        public async Task<IActionResult> UploadUsertImage(IFormFile files)
        {
            string response = await blobStorageService.UploadUserAvatar(files);
            return Ok(response);
        }
        [HttpPost("edit-user-image/{Id}")]
        public async Task<IActionResult> EditUploadedUserImage(IFormFile files, int Id)
        {
            string response = await blobStorageService.EditUploadedUserImage(files, Id);
            return Ok(response);
        }
    }
}
