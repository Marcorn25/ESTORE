using ESTORE.Shared.DTO.Order;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.OrderServices
{
    public interface IOrderService
    {
        Task<GeneralResponse> AddOrderFromCart(OrderDTO order);
        Task<DataResponse<List<OrderDTO>>> GetAllUserOrder(string status);
        Task<GeneralResponse> CancelUserOrder(int Id);

        //Admin
        Task<DataResponse<List<OrderDTO>>> GetAllCustomerOrder(string status);

        Task<GeneralResponse> UpdateOrderStatus(OrderDTO order);
    }
}
