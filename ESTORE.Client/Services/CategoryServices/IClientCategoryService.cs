using ESTORE.Shared.DTO.Product;

namespace ESTORE.Client.Services.CategoryServices
{
    public interface IClientCategoryService
    {
        Task AddCategory(CategoryDTO category);
        Task<List<CategoryDTO>?> GetAllCategory();
        Task DeleteCategory(int Id);
        Task UpdateCategory(int Id, CategoryDTO category);

    }
}
