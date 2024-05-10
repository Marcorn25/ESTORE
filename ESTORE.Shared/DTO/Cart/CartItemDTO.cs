using ESTORE.Shared.DTO.Product;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Shared.DTO.Cart
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }   
        public UserDetailsDTO? User { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public float Subtotal { get; set; }
    }
}
