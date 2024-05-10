using ESTORE.Shared.DTO.Dashboard;

namespace ESTORE.Client.Services.DashboardServices
{
    public interface IClientDashboardService
    {
        Task<DashboardDTO?> GetAdminStat();
    }
}
