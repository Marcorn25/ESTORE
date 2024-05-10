using System.ComponentModel.DataAnnotations;

namespace ESTORE.Shared.DTO.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public float Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public CategoryDTO? ProductCategory { get; set; }
    }
}
