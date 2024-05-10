using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Shared.DTO.Product;
using static ESTORE.Server.Services.ResponseServices.Response;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ESTORE.Server.Services.DataServices;

namespace ESTORE.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext context;
        private readonly IDataService dataService;

        public CategoryService(DataContext context, IDataService dataService)
        {
            this.context = context;
            this.dataService = dataService;
        }

        public async Task<GeneralResponse> AddCategory(CategoryDTO category)
        {
            if (category == null)
                return new GeneralResponse("Category is Empty", HttpStatusCode.BadRequest);

            var newCategory = new Category()
            {
                Name = category.Name,
            };

            context.Categories.Add(newCategory);
            await context.SaveChangesAsync();
            return new GeneralResponse("Category is successfully Added");

        }
        public async Task<DataResponse<List<CategoryDTO>>> GetAllCategory()
        {
            var response = await context.Categories.ToListAsync();
            if (response == null)
                return new DataResponse<List<CategoryDTO>>(null!, "No existing category", HttpStatusCode.NotFound);

            var categories = response.Select(p => dataService.CastToCategoryDTO(p)).ToList();

            return new DataResponse<List<CategoryDTO>>(categories, "Category Fetched");

        }
        public async Task<GeneralResponse> DeleteCategory(int Id)
        {
            var response = await context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
            if (response == null)
                return new GeneralResponse("No existing category", HttpStatusCode.NotFound);

            context.Categories.Remove(response);
            await context.SaveChangesAsync();

            return new GeneralResponse("Category Deleted");
        }
        public async Task<GeneralResponse> UpdateCategory(int Id, CategoryDTO category)
        {
            var response = await context.Categories.FirstOrDefaultAsync(c => c.Id == Id);
            if (response == null)
                return new GeneralResponse("No existing category", HttpStatusCode.NotFound);

            response.Name = category.Name;
            await context.SaveChangesAsync();

            return new GeneralResponse("Category Updated");
        }
    }
}
