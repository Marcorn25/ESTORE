namespace ESTORE.Server.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float Subtotal { get; set; }
    }
}
