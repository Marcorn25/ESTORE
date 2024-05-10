using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Shared.DTO.Product;
using static ESTORE.Server.Services.ResponseServices.Response;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ESTORE.Server.Services.BlobStorageServices;
using ESTORE.Server.Services.DataServices;

namespace ESTORE.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {

        private readonly DataContext context;
        private readonly IDataService dataService;
        private readonly IBlobStorageService blobStorageService;
        public ProductService(DataContext context, IDataService dataService, IBlobStorageService blobStorageService)
        {
            this.context = context;
            this.dataService = dataService;
            this.blobStorageService = blobStorageService;
        }

        public async Task<GeneralResponse> AddProduct(ProductDTO product)
        {
            if (product == null)
                return new GeneralResponse("Product is Empty", HttpStatusCode.BadRequest);

            var category = await context.Categories.FindAsync(product.ProductCategory.Id);

            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                ProductCategory = category,
            };

            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
            return new GeneralResponse("Product is successfully Added");
        }

        public async Task<GeneralResponse> DeleteProduct(int Id)
        {
            var product = context.Products.FirstOrDefault(x => x.Id == Id);
            if (product == null)
                return new GeneralResponse("Product Not Found", HttpStatusCode.NotFound);

            var deleteProductBlob = await blobStorageService.DeleteFileToBlobAsync(product.ImageUrl);
            if (!deleteProductBlob)
                return new GeneralResponse("Error Occured", HttpStatusCode.BadRequest);

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return new GeneralResponse("Product Deleted Successfully");
        }

        public async Task<DataResponse<List<ProductDTO>>> GetAllProducts()
        {
            var products = await context.Products.Include(product => product.ProductCategory).ToListAsync();
            if (products == null)
                return new DataResponse<List<ProductDTO>>(null!, "No Data Found", HttpStatusCode.NotFound);

            var productList = products.Select(p => dataService.CastToProductDTO(p)).ToList();
            return new DataResponse<List<ProductDTO>>(productList, "Products Fetched");
        }

        public async Task<DataResponse<ProductDTO>> GetProductById(int Id)
        {
            var product = await context.Products.Include(product => product.ProductCategory).FirstOrDefaultAsync(x => x.Id == Id);
            if (product == null)
                return new DataResponse<ProductDTO>(null!, "No Data Found", HttpStatusCode.NotFound);

            var productDTO = dataService.CastToProductDTO(product);

            return new DataResponse<ProductDTO>(productDTO, "Data Fetched");
        }

        public async Task<GeneralResponse> UpdateProduct(ProductDTO Product, int Id)
        {
            var response = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (response == null)
                return new GeneralResponse("No Data Found", HttpStatusCode.NotFound);

            response.Name = Product.Name;
            response.Description = Product.Description;
            response.Price = Product.Price;
            response.Quantity = Product.Quantity;
            response.ImageUrl = Product.ImageUrl;
            response.CategoryId = Product.CategoryId;

            await context.SaveChangesAsync();
            return new GeneralResponse("Product Updated Successfully");
        }
    }
}

