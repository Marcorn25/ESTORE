using ESTORE.Shared.DTO.Cart;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.CartServices
{
    public interface ICartService
    {
        Task<GeneralResponse> AddCartItemToCart(CartItemDTO cartItem);
        Task<DataResponse<List<CartItemDTO>>> GetAllItemsFromCart(int userId);
        Task<GeneralResponse> UpdateCartItems(List<CartItemDTO> cartItems);
        Task<GeneralResponse> RemoveCartItem(int Id);

    }
}
