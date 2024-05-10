using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.DataServices;
using ESTORE.Shared.DTO.Dashboard;
using ESTORE.Shared.DTO.Product;
using Microsoft.EntityFrameworkCore;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext context;
        private readonly IDataService dataService;
        public DashboardService(DataContext context, IDataService dataService)
        {
            this.context = context;
            this.dataService = dataService;
        }

        public async Task<DataResponse<DashboardDTO>> GetAdminDashboard()
        {
            DashboardDTO dashboardStat = new DashboardDTO();

            dashboardStat.NoOfProducts = await context.Products.CountAsync();
            dashboardStat.NoOfUsers = await context.Users.CountAsync();

            var allOrders = await context.Orders.ToListAsync();

            dashboardStat.SalesProduct = new DashboardDTO.Sales()
            {
                NoOfProcessing = allOrders.Count(o => o.Status.ToString() == "PROCESSING"),
                NoOfPreparing = allOrders.Count(o => o.Status.ToString() == "PREPARING"),
                NoOfDelivery = allOrders.Count(o => o.Status.ToString() == "DELIVERY"),
                NoOfDelivered = allOrders.Count(o => o.Status.ToString() == "DELIVERED"),
                NoOfCancelled = allOrders.Count(o => o.Status.ToString() == "CANCELLED")
            };

            var mostOrderedProducts = await GetTop5MostOrderedProducts();

            dashboardStat.mostOrderedProducts = mostOrderedProducts.Select(product => dataService.CastToProductDTO(product)).ToList();

            return new DataResponse<DashboardDTO>(dashboardStat, "Data Fetched");
        }

        public async Task<List<Product>> GetTop5MostOrderedProducts()
        {
            var topProducts = await context.OrderItems
                .Include(orderItem => orderItem.Product)
                .ThenInclude(product => product.ProductCategory)
                .GroupBy(od => od.Product.Id)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalOrders = group.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(5)
                .ToListAsync();

            var productIds = topProducts.Select(tp => tp.ProductId).ToList();
            var topProductsWithDetails = await context.Products
                .Include(product => product.ProductCategory)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            return topProductsWithDetails;
        }
    }
}
