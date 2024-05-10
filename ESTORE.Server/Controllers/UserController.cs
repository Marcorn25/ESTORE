using ESTORE.Server.Services.UserServices;
using ESTORE.Shared.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESTORE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDetailsDTO>> GetAuthenticatedUserDetails()
        {
            var response = await userService.GetAuthenticatedUserDetails();

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Data);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }

        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var response = await userService.GetUserAddress();

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Data);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }

        [HttpPut("address")]
        public async Task<ActionResult> UpdateUserAddress(AddressDTO address)
        {
            var response = await userService.UpdateUserAddress(address);

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Message);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }

        [HttpPut("details/{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserDetails(UserDetailsDTO user, int id)
        {
            var response = await userService.UpdateUserDetails(user, id);

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Message);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }

        //Admin
        [HttpGet("admin")]
        public async Task<ActionResult<List<UserDetailsDTO>>> GetAllUserController()
        {
            var response = await userService.GetAllUsers();

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Data);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }

        [HttpPut("admin/update-role")]
        public async Task<ActionResult> UpdateUserRole(UserDetailsDTO user)
        {
            var response = await userService.UpdateUserRole(user);

            switch (response.ResponseCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Message);
                case HttpStatusCode.NotFound:
                    return NotFound(response.Message);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.Message);
                default:
                    return BadRequest("Error Occured, Try Again Later");
            }
        }


    }
}
