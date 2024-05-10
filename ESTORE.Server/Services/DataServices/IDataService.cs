using ESTORE.Server.Models;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.ChatMessage;
using ESTORE.Shared.DTO.Order;
using ESTORE.Shared.DTO.Product;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Server.Services.DataServices
{
    public interface IDataService
    {
        AddressDTO CastToAddressDTO(Address address);
        Address CastToAddress(AddressDTO addressDTO);
        UserDetailsDTO CastToUserDetailsDTO(User user);
        ProductDTO CastToProductDTO(Product product);
        CategoryDTO CastToCategoryDTO(Category category);
        ChatMessageDTO CastToChatMessageDTO(ChatMessage chatMessage);
        CartItemDTO CastToCartItemDTO(CartItem cartItem);
        OrderItemDTO CastToOrderItemDTO(OrderItem orderItem);
        OrderDTO CastToOrderDTO(Order order);


    }
}
