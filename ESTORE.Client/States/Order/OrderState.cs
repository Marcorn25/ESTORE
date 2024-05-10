using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.Order;
using ESTORE.Shared.DTO.User;
using ESTORE.Shared.Enum;

namespace ESTORE.Client.States.Order
{
    public class OrderState
    {
        public List<OrderItemDTO>? OrderItems { get; set; }
        public float Subtotal { get; set; }
        public float ShippingFee { get; set; }
        public float Total { get; set; }
        public AddressDTO? ShippingAddress { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public PaymentType? paymentType { get; set; } = PaymentType.CashOnDelivery;

    }
}
