using ESTORE.Client.Pages.Admin.Orders;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.CartServices
{
    public class ClientCartService : IClientCartService
    {
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        private readonly ISnackbar snackbar;
        public ClientCartService(HttpClient httpClient, NavigationManager navigationManager, ISnackbar snackbar)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            this.snackbar = snackbar;
        }

        public async Task AddCartItemToCart(CartItemDTO cartItem)
        {
            try
            {

                var response = await httpClient.PostAsJsonAsync("api/cart/add-item", cartItem);
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
        public async Task<List<CartItemDTO>?> GetItemsFromCart(int Id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<CartItemDTO>>($"api/Cart/{Id}");
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

        public async Task<int> RemoveCartItem(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/cart/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Success);
                    return 1;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add(result, Severity.Error);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
                return 0;
            }
        }

    }
}
