using ESTORE.Client.Services.AuthServices;
using ESTORE.Client.Services.UserService;
using ESTORE.Client.States.Order;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;

namespace ESTORE.Client.Services.OrderService
{
    public class ClientOrderService : IClientOrderService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;
        private readonly IClientUserService userService;
        private readonly IClientAuthService authService;
        private OrderState orderState;
        private readonly NavigationManager navigationManager;
        public ClientOrderService(HttpClient httpClient, ISnackbar snackbar, IClientUserService userService, OrderState orderState, NavigationManager navigationManager, IClientAuthService authService)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
            this.userService = userService;
            this.orderState = orderState;
            this.navigationManager = navigationManager;
            this.authService = authService;
        }

        public async Task AddOrderItem(HashSet<CartItemDTO> selectedItems, float Subtotal, float Total, float ShippingFee)
        {
            var newOrder = new OrderState()
            {
                OrderItems = new List<OrderItemDTO>(),
                CartItems = new List<CartItemDTO>(),
                Subtotal = Subtotal,
                ShippingFee = ShippingFee,
                Total = Total,
            };

            foreach (var item in selectedItems)
            {
                var cartItem = new CartItemDTO()
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Subtotal = Subtotal,
                };

                var orderItem = new OrderItemDTO()
                {
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal,
                };

                newOrder.OrderItems.Add(orderItem);
                newOrder.CartItems.Add(cartItem);
            }
            orderState.OrderItems = newOrder.OrderItems;
            orderState.CartItems = newOrder.CartItems;
            orderState.Subtotal = newOrder.Subtotal;
            orderState.Total = newOrder.Total;
            orderState.ShippingFee = newOrder.ShippingFee;
        }

        public async Task AddOrderFromCart()
        {
            try
            {
                OrderDTO order = new OrderDTO();

                var user = await userService.GetUserDetails();
                if (user != null)
                {
                    order = new OrderDTO()
                    {
                        OrderItems = orderState.OrderItems,
                        UserId = user.Id,
                        User = user,
                        Total = orderState.Total,
                        Subtotal= orderState.Subtotal,
                        ShippingFee = orderState.ShippingFee,
                        ShippingAddress = orderState.ShippingAddress,
                        paymentType = orderState.paymentType,
                    };

                    var response = await httpClient.PostAsJsonAsync("api/Order", order);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var updateCart = await httpClient.PostAsJsonAsync("api/cart/update-cart",  orderState.CartItems.ToList());
                        navigationManager.NavigateTo("/order-success");
                    }
                    else
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        snackbar.Add(result, Severity.Error);
                    }
                }
                else
                {
                    snackbar.Add("An error occurred: Please Try Again.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }

        public async Task<List<OrderDTO>?> GetAllUserOrder(string status)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<OrderDTO>>($"api/Order?status={status}");
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

        public async Task<int> CancelUserOrder(OrderDTO order)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Order/{order.Id}");
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

        public async Task AddDirecOrderItem(OrderItemDTO orderItem, float Total, float Subtotal, float ShippingFee)
        {
            var newOrder = new OrderState()
            {
                OrderItems = new List<OrderItemDTO>(),
                CartItems = new List<CartItemDTO>(),
                Subtotal = Subtotal,
                ShippingFee = ShippingFee,
                Total = Total,
            };


                var AddOrderItem = new OrderItemDTO()
                {
                    Product = orderItem.Product,
                    Quantity = orderItem.Quantity,
                    Subtotal = orderItem.Subtotal,
                };

            newOrder.OrderItems.Add(AddOrderItem);

            orderState.OrderItems = newOrder.OrderItems;
            orderState.CartItems = newOrder.CartItems;
            orderState.Subtotal = newOrder.Subtotal;
            orderState.Total = newOrder.Total;
            orderState.ShippingFee = newOrder.ShippingFee;
        }
        public async Task AddDirectOrder(OrderItemDTO orderItem)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/order", orderItem);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    navigationManager.NavigateTo("/order-success");
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

        //Admin
        public async Task<List<OrderDTO>?> GetAllCustomerOrders(string status)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<OrderDTO>>($"api/Order/all?status={status}");
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
        public async Task UpdateOrderStatus(OrderDTO order)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync("api/Order/update", order);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add("Update Status Successfully", Severity.Success);
                    return;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    snackbar.Add(errorMessage, Severity.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }

    }
}
