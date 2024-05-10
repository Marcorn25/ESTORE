using ESTORE.Shared.DTO.Product;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<GeneralResponse> AddCategory(CategoryDTO category);
        Task<DataResponse<List<CategoryDTO>>> GetAllCategory();
        Task<GeneralResponse> DeleteCategory(int Id);
        Task<GeneralResponse> UpdateCategory(int Id, CategoryDTO category);
    }
}
