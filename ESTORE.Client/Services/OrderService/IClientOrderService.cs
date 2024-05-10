using ESTORE.Client.States.Order;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.Order;

namespace ESTORE.Client.Services.OrderService
{
    public interface IClientOrderService
    {
        Task AddOrderItem(HashSet<CartItemDTO> selectedItems, float Subtotal, float Total, float ShippingFee);
        Task AddOrderFromCart();
        Task<List<OrderDTO>?> GetAllUserOrder(string status);
        Task<int> CancelUserOrder(OrderDTO order);
        Task AddDirecOrderItem(OrderItemDTO orderItem, float Total, float Subtotal, float ShippingFee);
        Task AddDirectOrder(OrderItemDTO orderItem);


        //Admin 
        Task<List<OrderDTO>?> GetAllCustomerOrders(string status);
        Task UpdateOrderStatus(OrderDTO order);

    }
}
