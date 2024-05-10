using ESTORE.Shared.Enum;

namespace ESTORE.Server.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Address ShippingAddress { get; set; }
        public User User { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public float Total { get; set; }
        public float Subtotal { get; set; }
        public float ShipppingFee { get; set; }
        public PaymentType PaymentType { get; set; } = PaymentType.CashOnDelivery;
        public OrderStatus? Status { get; set; } = OrderStatus.PROCESSING;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
