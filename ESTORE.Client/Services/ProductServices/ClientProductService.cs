using ESTORE.Shared.DTO.Payload;
using ESTORE.Shared.DTO.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.ProductServices
{
    public class ClientProductService : IClientProductService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;
        private readonly NavigationManager navigationManager;

        public ClientProductService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
            this.navigationManager = navigationManager;
        }

        public async Task AddProduct(ProductDTO Product)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/Product", Product);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
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

        public async Task DeleteProduct(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Product/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
                    navigationManager.NavigateTo("/admin/products");
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

        public async Task<List<ProductDTO>?> GetAllProducts()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<ProductDTO>>("api/Product");
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

        public async Task<ProductDTO?> GetProductById(int Id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ProductDTO>($"api/Product/{Id}");
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

        public async Task UpdateProduct(ProductDTO Product, int Id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Product/{Id}", Product);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
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

