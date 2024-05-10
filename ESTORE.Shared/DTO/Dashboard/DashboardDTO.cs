using ESTORE.Shared.DTO.Product;

namespace ESTORE.Shared.DTO.Dashboard
{
    public class DashboardDTO
    {
        public int NoOfUsers { get; set; }
        public int NoOfProducts { get; set; }

        public class Sales()
        {
            public int NoOfProcessing { get; set; }
            public int NoOfPreparing { get; set; }
            public int NoOfDelivery { get; set; }
            public int NoOfDelivered { get; set; }
            public int NoOfCancelled { get; set; }
        }
        public Sales SalesProduct { get; set; }

        public List<ProductDTO>? mostOrderedProducts { get; set; }
    }
}
