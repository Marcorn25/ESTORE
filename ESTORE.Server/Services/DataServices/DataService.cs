using ESTORE.Server.Models;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.ChatMessage;
using ESTORE.Shared.DTO.Order;
using ESTORE.Shared.DTO.Product;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Server.Services.DataServices
{
    public class DataService : IDataService
    {
        public AddressDTO CastToAddressDTO(Address address)
        {
            return new AddressDTO()
            {
                Street = address.Street,
                City = address.City,
                Barangay = address.Barangay,
                ZipCode = address.ZipCode,
                Country = address.Country,
            };
        }

        public Address CastToAddress(AddressDTO addressDTO)
        {
            return new Address()
            {
                Street = addressDTO.Street,
                City = addressDTO.City,
                Barangay = addressDTO.Barangay,
                ZipCode = addressDTO.ZipCode,
                Country = addressDTO.Country,
            };
        }

        public UserDetailsDTO CastToUserDetailsDTO(User user)
        {
            return new UserDetailsDTO()
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = user.Role,
                AvatarURL = user.AvatarURL,
                UserName = user.UserName,
                HomeAddress = CastToAddressDTO(user.HomeAddress)
            };
        }

        public ProductDTO CastToProductDTO(Product product)
        {
            return new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                ProductCategory = CastToCategoryDTO(product.ProductCategory)
            };
        }

        public CategoryDTO CastToCategoryDTO(Category category)
        {
            return new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public ChatMessageDTO CastToChatMessageDTO(ChatMessage chatMessage)
        {
            return new ChatMessageDTO()
            {
                Id = chatMessage.Id,
                Content = chatMessage.Content,
                TimeStamp = chatMessage.TimeStamp,
                User = new UserChatMessageDTO
                {
                    Id = chatMessage.User.Id,
                    FullName = chatMessage.User.FullName,
                    AvatarURL = chatMessage.User.AvatarURL,
                    Role = chatMessage.User.Role,
                    UserName = chatMessage.User.UserName,
                }
            };
        }

        public CartItemDTO CastToCartItemDTO(CartItem cartItem)
        {
            return new CartItemDTO()
            {
                Id = cartItem.Id,
                UserId = cartItem.User.Id,
                User = CastToUserDetailsDTO(cartItem.User),
                Product = CastToProductDTO(cartItem.Product),
                Quantity = cartItem.Quantity,
                Subtotal = cartItem.Subtotal,
            };
        }

        public OrderItemDTO CastToOrderItemDTO(OrderItem orderItem)
        {
            return new OrderItemDTO()
            {
                Id = orderItem.Id,
                OrderId = orderItem.Order.Id,
                ProductId = orderItem.Product.Id,
                Product = CastToProductDTO(orderItem.Product),
                Quantity = orderItem.Quantity,
                Subtotal= orderItem.Subtotal,
            };
        }

        public OrderDTO CastToOrderDTO(Order order)
        {
            return new OrderDTO()
            {
                Id = order.Id,
                UserId = order.User.Id,
                User = CastToUserDetailsDTO(order.User),
                DateOrdered = order.DateOrdered,
                OrderItems = order.OrderItems.Select(orderItem => CastToOrderItemDTO(orderItem)).ToList(),
                Subtotal = order.Subtotal,
                Total = order.Total,
                ShippingFee = order.ShipppingFee,
                ShippingAddress = CastToAddressDTO(order.ShippingAddress),
                Status = order.Status,
                paymentType = order.PaymentType,
            };
        }
    }
}

