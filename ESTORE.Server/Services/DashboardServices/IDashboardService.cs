using ESTORE.Shared.DTO.Dashboard;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<DataResponse<DashboardDTO>> GetAdminDashboard();
    }
}
