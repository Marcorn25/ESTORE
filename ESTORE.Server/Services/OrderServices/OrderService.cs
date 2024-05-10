using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.UserServices;
using ESTORE.Shared.DTO.Order;
using static ESTORE.Server.Services.ResponseServices.Response;
using System.Net;
using ESTORE.Server.Services.DataServices;
using Microsoft.EntityFrameworkCore;
using ESTORE.Shared.DTO.Product;
using ESTORE.Shared.Enum;
using System.Security.Claims;
using ESTORE.Shared.DTO.User;

namespace ESTORE.Server.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly IDataService dataService;
        private readonly IHttpContextAccessor contextAccessor;
        public OrderService(DataContext context, IUserService userService, IDataService dataService, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.dataService = dataService;
            this.contextAccessor = contextAccessor;
        }

        public async Task<GeneralResponse> AddOrderFromCart(OrderDTO order)
        {
            if (order == null)
                return new GeneralResponse("Order is Empty", HttpStatusCode.BadRequest);

            var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));


        var newOrder = new Order()
            {
                OrderItems = new List<OrderItem>(),
                ShippingAddress = dataService.CastToAddress(order.ShippingAddress),
                Total = order.Total,
                Subtotal = order.Subtotal,
                ShipppingFee = order.ShippingFee,
                User = user,
                PaymentType = (PaymentType)order.paymentType,
            };

            foreach (var item in order.OrderItems)
            {
                var product = await context.Products.FindAsync(item.Product.Id);
                var orderItem = new OrderItem()
                {
                    Order = newOrder,
                    Product = product,
                    Quantity = item.Quantity,
                    Subtotal = item.Subtotal,
                };
                product.Quantity -= item.Quantity;
                newOrder.OrderItems.Add(orderItem);
                context.OrderItems.Add(orderItem);
            }

            context.Orders.Add(newOrder);
            await context.SaveChangesAsync();
            return new GeneralResponse("Order Successfully Added");
        }

        public async Task<DataResponse<List<OrderDTO>>> GetAllUserOrder(string status)
        {
            var userId = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int parsedUserId))
            {
                throw new ArgumentException("Invalid userId.");
            }

            OrderStatus parsedStatus;
            if (!Enum.TryParse(status, true, out parsedStatus))
            {
                throw new ArgumentException("Invalid order status.");
            }

            var orders = await context.Orders
                          .Include(order => order.User)
                          .ThenInclude(user => user.HomeAddress)
                          .Where(order => order.User.Id == parsedUserId && order.Status == parsedStatus)
                          .Include(order => order.ShippingAddress)
                          .Include(order => order.OrderItems)
                          .ThenInclude(orderItem => orderItem.Product)
                          .ThenInclude(product => product.ProductCategory)
                          .ToListAsync();

            if (orders == null)
                return new DataResponse<List<OrderDTO>>(null!, "No Data Found", HttpStatusCode.NotFound);


            var userOrder = orders.Select(item => dataService.CastToOrderDTO(item)).ToList();

            return new DataResponse<List<OrderDTO>>(userOrder!, "Products Fetched");
        }

        public async Task<GeneralResponse> CancelUserOrder(int Id)
        {
            var order = await context.Orders
                                        .Include(order => order.ShippingAddress)
                                        .Include(order => order.User)
                                        .Include(order => order.OrderItems)
                                        .ThenInclude(orderItem => orderItem.Product)
                                        .ThenInclude(product => product.ProductCategory)
                                        .FirstOrDefaultAsync(order => order.Id == Id);
            if (order == null)
                return new GeneralResponse("No Order Found", HttpStatusCode.NotFound);

            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Product.Quantity += orderItem.Quantity;
            }

            order.Status = OrderStatus.CANCELLED;
            await context.SaveChangesAsync();
            return new GeneralResponse("Order Cancelled");
        }

        //Admin
        public async Task<DataResponse<List<OrderDTO>>> GetAllCustomerOrder(string status)
        {
            OrderStatus parsedStatus;
            if (!Enum.TryParse(status, true, out parsedStatus))
            {
                throw new ArgumentException("Invalid order status.");
            }
            var orders = await context.Orders
                                        .Include(order => order.User)
                                          .ThenInclude(user => user.HomeAddress)
                                          .Where(order =>  order.Status == parsedStatus)
                                          .Include(order => order.ShippingAddress)
                                          .Include(order => order.OrderItems)
                                          .ThenInclude(orderItem => orderItem.Product)
                                          .ThenInclude(product => product.ProductCategory)
                                          .ToListAsync();

            if (orders == null)
                return new DataResponse<List<OrderDTO>>(null!, "No Data Found", HttpStatusCode.NotFound);


            var userOrder = orders.Select(item => dataService.CastToOrderDTO(item)).ToList();

            return new DataResponse<List<OrderDTO>>(userOrder!, "Products Fetched");
        }


        public async Task<GeneralResponse> UpdateOrderStatus(OrderDTO order)
        {
            var getOrder = await context.Orders.FindAsync(order.Id);
            if (getOrder == null)
                return new GeneralResponse("No Order Found", HttpStatusCode.NotFound);

            getOrder.Status = order.Status;

            context.Orders.Update(getOrder);
            await context.SaveChangesAsync();
            return new GeneralResponse("User Details Updated Successfully", HttpStatusCode.OK);
        }

    }
}
