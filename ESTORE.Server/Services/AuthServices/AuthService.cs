using ESTORE.Server.Data;
using ESTORE.Server.Models;
using ESTORE.Server.Services.DataServices;
using ESTORE.Shared.DTO.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static ESTORE.Server.Services.ResponseServices.Response;

namespace ESTORE.Server.Services.AuthServices
{
    public class AuthService :IAuthService
    {
        private readonly IDataService dataService;
        private readonly DataContext context;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor contextAccessor;
        private User user = new User();
        public AuthService(IDataService dataService, DataContext context, IConfiguration config, IHttpContextAccessor contextAccessor)
        {
            this.dataService = dataService;
            this.context = context;
            this.config = config;
            this.contextAccessor = contextAccessor;
        }

        public async Task<LoginResponse> CreateAccount(RegisterDTO newUser)
        {
            if (newUser is null) return new LoginResponse(null!, "Model is empty", HttpStatusCode.BadRequest);

            var getExistingEmail = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            if (getExistingEmail is not null) return new LoginResponse(null!, "Email is already Registered", HttpStatusCode.BadRequest);

            var getExistingUsername = await context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName);
            if (getExistingUsername is not null) return new LoginResponse(null!, "Username already Exist", HttpStatusCode.BadRequest);

            CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.FullName = newUser.FullName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = newUser.Email;
            user.UserName = newUser.UserName;
            user.HomeAddress = new Address();

            context.Users.Add(user);
            var saveUser = await context.SaveChangesAsync();
            if (saveUser == 0)
                return new LoginResponse(null!, "Error occured. Try again later...", HttpStatusCode.BadRequest);

            string token = CreateTokenAsync(user);
  
            return new LoginResponse(token, "User Added Succesfully", HttpStatusCode.OK);
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO is null) return new LoginResponse(null!, "Error occured, Login input is null", HttpStatusCode.BadRequest);

            var getUser = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.EmailOrUsername || u.UserName == loginDTO.EmailOrUsername);
            if (getUser is null)
                return new LoginResponse(null!, "User not found", HttpStatusCode.NotFound);

            if (!VerifyPasswordHash(loginDTO.Password, getUser.PasswordHash, getUser.PasswordSalt))
                return new LoginResponse(null!, "Password Incorrect", HttpStatusCode.BadRequest);

            string token = CreateTokenAsync(getUser);

            return new LoginResponse(token, "Login Successfully", HttpStatusCode.OK);
        }

        public async Task<GeneralResponse> UpdatePassword(ChangePasswordDTO passwordDTO)
        {
            var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;
            var getUser = await context.Users.Include(u => u.HomeAddress).FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
            if (getUser == null)
                return new GeneralResponse("No User Found", HttpStatusCode.NotFound);

            CreatePasswordHash(passwordDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            getUser.PasswordHash = passwordHash;
            getUser.PasswordSalt = passwordSalt;

            context.Users.Update(getUser);
            await context.SaveChangesAsync();
            return new GeneralResponse("Password Updated Successfully", HttpStatusCode.OK);
        }

        //Creating Password Hash and Salt
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Verify PasswordHash
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        //Create Token
        public string CreateTokenAsync(User user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()!),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(

                   claims: claims,
                   expires: DateTime.UtcNow.AddDays(1),
                   signingCredentials: credentials

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
    }
}
