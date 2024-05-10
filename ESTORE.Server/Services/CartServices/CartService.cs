using ESTORE.Client.Pages.User;
using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.DataServices;
using ESTORE.Shared.DTO.Cart;
using ESTORE.Shared.DTO.Product;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly DataContext context;
        private readonly IDataService dataService;

        public CartService(DataContext context, IDataService dataService)
        {
            this.context = context;
            this.dataService = dataService;
        }

        public async Task<GeneralResponse> AddCartItemToCart(CartItemDTO cartItem)
        {
            if (cartItem == null)
                return new GeneralResponse("Cart Item is empty", HttpStatusCode.BadRequest);

            var product = await context.Products
                                        .Include(ci => ci.ProductCategory)
                                        .FirstOrDefaultAsync(i => i.Id == cartItem.Product.Id);
            if (product == null)
                return new GeneralResponse("Product not found", HttpStatusCode.NotFound);

            var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == cartItem.UserId);
            if(user == null)
                return new GeneralResponse("User not found", HttpStatusCode.NotFound);

            var cartItems = await context.CartItems
               .Include(user => user.User)
               .Include(product => product.Product)
               .Where(u => u.User.Id == cartItem.UserId)
               .ToListAsync();

            var existingItem = cartItems.Find(items => cartItem.Product?.Id == items.Product?.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                existingItem.Subtotal += cartItem.Subtotal;
                await context.SaveChangesAsync();

                return new GeneralResponse("Product successfully added");
            }
            else
            {
                var newCartItem = new CartItem()
                {
                    User = user,
                    Product = product,
                    Quantity = cartItem.Quantity,
                    Subtotal = cartItem.Subtotal,
                };

                context.CartItems.Add(newCartItem);
                await context.SaveChangesAsync();

                return new GeneralResponse("Product successfully added");
            }

        }

        public async Task<GeneralResponse> RemoveCartItem(int Id)
        {
            try 
            {
                var cartItem = await context.CartItems.FirstOrDefaultAsync(item => item.Id == Id);
                if (cartItem == null)
                    return new GeneralResponse("Item not Found", HttpStatusCode.NotFound);

                context.Remove(cartItem);
                await context.SaveChangesAsync();
                return new GeneralResponse("Item Removed!");
            }
            catch
            {
                return new GeneralResponse("Error Occurred", HttpStatusCode.BadRequest);
            }
        }

        public async Task<DataResponse<List<CartItemDTO>>> GetAllItemsFromCart(int userId)
        {
            var cartItems = await context.CartItems
                .Include(user => user.User)
                .ThenInclude(user => user.HomeAddress)
                .Include(product => product.Product)
                .ThenInclude(product => product.ProductCategory)
                .Where(u => u.User.Id == userId)
                .ToListAsync();

            if (cartItems == null)
                return new DataResponse<List<CartItemDTO>>(null!, "No Data Found", HttpStatusCode.NotFound);

            var cartItemsList = cartItems.Select(ci => dataService.CastToCartItemDTO(ci)).ToList();

            return new DataResponse<List<CartItemDTO>>(cartItemsList!, "Products Fetched");
        }

        public async Task<GeneralResponse> UpdateCartItems(List<CartItemDTO> cartItems)
        {
            try
            {
                foreach (var item in cartItems)
                {
                    var itemToDelete = await context.CartItems.FindAsync(item.Id);
                    if (itemToDelete != null)
                        context.CartItems.Remove(itemToDelete);

                }

                await context.SaveChangesAsync();

                return new GeneralResponse("Cart Updated");
            }
            catch
            {
                return new GeneralResponse("Error Occurred", HttpStatusCode.BadRequest);
            }

        }
    }
}
