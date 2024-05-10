using ESTORE.Shared.DTO.Product;

namespace ESTORE.Shared.DTO.Order
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderDTO? Order { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }
        public float Subtotal { get; set; }
    }
}
