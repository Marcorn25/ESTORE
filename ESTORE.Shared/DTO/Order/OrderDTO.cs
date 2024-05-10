using ESTORE.Shared.DTO.User;
using ESTORE.Shared.Enum;

namespace ESTORE.Shared.DTO.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDetailsDTO? User { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public List<OrderItemDTO>? OrderItems { get; set; }
        public float Subtotal { get; set; }
        public float ShippingFee { get; set; }
        public float Total { get; set; }
        public OrderStatus? Status { get; set; } = OrderStatus.PROCESSING;
        public PaymentType? paymentType { get; set; } = PaymentType.CashOnDelivery;
        public AddressDTO? ShippingAddress { get; set; }
    }
}
