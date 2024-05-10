using ESTORE.Shared.DTO.Cart;

namespace ESTORE.Client.Services.CartServices
{
    public interface IClientCartService
    {
        Task AddCartItemToCart(CartItemDTO cartItem);
        Task<List<CartItemDTO>?> GetItemsFromCart(int Id);
        Task<int> RemoveCartItem(int Id);

    }
}
