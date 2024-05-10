using ESTORE.Server.Data;
using ESTORE.Server.Services.DataServices;
using ESTORE.Shared.DTO.User;
using static ESTORE.Server.Services.ResponseServices.Response;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ESTORE.Shared.Enum;

namespace ESTORE.Server.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly DataContext context;
        private readonly IDataService dataService;

        public UserService(IHttpContextAccessor contextAccessor, DataContext context, IDataService dataService)
        {
            this.contextAccessor = contextAccessor;
            this.context = context;
            this.dataService = dataService;
        }

        public async Task<DataResponse<UserDetailsDTO>> GetAuthenticatedUserDetails()
        {
            if (contextAccessor.HttpContext != null)
            {
                var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
                if (user != null)
                    return new DataResponse<UserDetailsDTO>(dataService.CastToUserDetailsDTO(user), "User Result");
                return new DataResponse<UserDetailsDTO>(null!, "User Result", HttpStatusCode.BadRequest);
            }
            return new DataResponse<UserDetailsDTO>(null!, "User Not Authenticated", HttpStatusCode.BadRequest);
        }

        public async Task<DataResponse<AddressDTO>> GetUserAddress()
        {
            if (contextAccessor.HttpContext != null)
            {
                var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
                if (user != null)
                    return new DataResponse<AddressDTO>(dataService.CastToAddressDTO(user.HomeAddress), "User Result");
                return new DataResponse<AddressDTO>(null!, "User Result", HttpStatusCode.BadRequest);
            }
            return new DataResponse<AddressDTO>(null!, "User Not Authenticated", HttpStatusCode.BadRequest);
        }

        public async Task<GeneralResponse> UpdateUserAddress(AddressDTO address)
        {
            if (contextAccessor.HttpContext != null)
            {
                var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
                var user = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
                if (user == null)
                    return new GeneralResponse("No User Found", HttpStatusCode.NotFound);

                user.HomeAddress.Street = address.Street;
                user.HomeAddress.City = address.City;
                user.HomeAddress.Barangay = address.Barangay;
                user.HomeAddress.ZipCode = address.ZipCode;
                user.HomeAddress.Country = address.Country;

                context.Users.Update(user);
                await context.SaveChangesAsync();
                return new GeneralResponse("User Details Updated Successfully", HttpStatusCode.OK);
            }
            return new GeneralResponse("User Not Authenticated", HttpStatusCode.BadRequest);
        }

        public async Task<GeneralResponse> UpdateUserDetails(UserDetailsDTO user, int Id)
        {
            var getUser = await context.Users.FindAsync(Id);
            if (getUser == null)
                return new GeneralResponse("No User Found", HttpStatusCode.NotFound);

            getUser.FullName = user.FullName;
            getUser.Email = user.Email;
            getUser.Role = (AccountRoles)user.Role;
            getUser.AvatarURL = user.AvatarURL;
            getUser.UserName = user.UserName;
            getUser.PhoneNumber = user.PhoneNumber;

            context.Users.Update(getUser);
            await context.SaveChangesAsync();
            return new GeneralResponse("User Details Updated Successfully", HttpStatusCode.OK);
        }

        //Admin
        public async Task<DataResponse<List<UserDetailsDTO>>> GetAllUsers()
        {
            var allUsers = await context.Users.Include(user=>user.HomeAddress).ToListAsync();
            if (allUsers == null)
                return new DataResponse<List<UserDetailsDTO>>(null!, "No User Found", HttpStatusCode.NotFound);

            var users = allUsers.Select(user => dataService.CastToUserDetailsDTO(user)).ToList();

            return new DataResponse<List<UserDetailsDTO>>(users, "Users Fetched");

        }

        public async Task<GeneralResponse> UpdateUserRole(UserDetailsDTO user)
        {
            var getUser = await context.Users.FindAsync(user.Id);
            if (getUser == null)
                return new GeneralResponse("No User Found", HttpStatusCode.NotFound);

            getUser.Role = (AccountRoles)user.Role;

            context.Users.Update(getUser);
            await context.SaveChangesAsync();
            return new GeneralResponse("User Details Updated Successfully", HttpStatusCode.OK);
        }
    }
}
