using ESTORE.Shared.DTO.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.CategoryServices
{
    public class ClientCategoryService : IClientCategoryService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;
        private readonly NavigationManager navigationManager;

        public ClientCategoryService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
            this.navigationManager = navigationManager;
        }

        public async Task AddCategory(CategoryDTO category)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/admin/category", category);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
                    navigationManager.NavigateTo("/admin/category");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }

        public async Task<List<CategoryDTO>?> GetAllCategory()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<CategoryDTO>>("api/admin/category");
                if (response != null)
                {
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
                return null;
            }
        }

        public async Task DeleteCategory(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/admin/category/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
                    navigationManager.NavigateTo("/admin/category");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }

        public async Task UpdateCategory(int Id, CategoryDTO category)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/admin/category/{Id}", category);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
                    navigationManager.NavigateTo("/admin/category");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }
    }
}
