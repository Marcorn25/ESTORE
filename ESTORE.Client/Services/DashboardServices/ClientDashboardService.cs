using ESTORE.Shared.DTO.Dashboard;
using MudBlazor;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.DashboardServices
{
    public class ClientDashboardService : IClientDashboardService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;

        public ClientDashboardService(HttpClient httpClient, ISnackbar snackbar)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
        }

        public async Task<DashboardDTO?> GetAdminStat()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<DashboardDTO>("api/Dashboard/admin");
                if (response != null)
                {
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
                return null;
            }
        }
    }
}
