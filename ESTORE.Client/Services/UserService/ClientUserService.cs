using ESTORE.Shared.DTO.User;
using MudBlazor;
using System.Net.Http;
using System.Net.Http.Json;

namespace ESTORE.Client.Services.UserService
{
    public class ClientUserService : IClientUserService
    {
        private readonly HttpClient httpClient;
        private readonly ISnackbar snackbar;

        public ClientUserService(HttpClient httpClient, ISnackbar snackbar)
        {
            this.httpClient = httpClient;
            this.snackbar = snackbar;
        }

        public async Task<UserDetailsDTO?> GetUserDetails()
        {
            var response = await httpClient.GetFromJsonAsync<UserDetailsDTO?>("api/user/");
            if (response != null)
                return response;
            return null;
        }

        public async Task<AddressDTO?> GetUserAddress()
        {
            var response = await httpClient.GetFromJsonAsync<AddressDTO?>("api/user/address");
            if (response != null)
                return response;
            return null;
        }

        public async Task<string?> UpdateUserAddress(AddressDTO address)
        {

            var response = await httpClient.PutAsJsonAsync($"api/user/address", address);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                snackbar.Add("Details Updated Successfully", Severity.Success);
                return null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }
        public async Task<string?> UpdateUserDetails(UserDetailsDTO user)
        {

            var response = await httpClient.PutAsJsonAsync($"api/user/details/{user.Id}", user);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                snackbar.Add("Details Updated Successfully", Severity.Success);
                return null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }
        public async Task<string?> ChangeUserPassword(ChangePasswordDTO password)
        {

            var response = await httpClient.PutAsJsonAsync($"api/Auth/change-password", password);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                snackbar.Add("Password Updated Successfully", Severity.Success);
                return null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }


        //Admin, SuperUser
        public async Task<List<UserDetailsDTO>?> GetAllUsers()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<UserDetailsDTO>>("api/user/admin");
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

        public async Task UpdateUserRole(UserDetailsDTO user)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync("api/user/admin/update-role", user);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    snackbar.Add("Updated Role Successfully", Severity.Success);
                    return;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    snackbar.Add(errorMessage, Severity.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An error occurred: " + ex.Message, Severity.Error);
            }
        }
    }
}
