﻿namespace ESTORE.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category? ProductCategory { get; set; }

        public List<CartItem>? CartItems { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
