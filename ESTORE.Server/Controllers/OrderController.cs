using ESTORE.Server.Services.OrderServices;
using ESTORE.Shared.DTO.Order;
using ESTORE.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESTORE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrderFromCartController(OrderDTO order)
        {
            var response = await orderService.AddOrderFromCart(order);

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

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllUserOrderController([FromQuery] string status = "PROCESSING")
        {
            var response = await orderService.GetAllUserOrder(status);

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

        [HttpDelete("{Id}")]
        public async Task<ActionResult> CancelUserOrder(int Id)
        {
            var response = await orderService.CancelUserOrder(Id);
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
        [HttpGet("all")]
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        public async Task<ActionResult<List<OrderDTO>>> GetAllCustomerOrdersController([FromQuery] string status = "PROCESSING")
        {
            var response = await orderService.GetAllCustomerOrder(status);

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

        [HttpPut("update")]
        [Authorize(Roles = "SUPERUSER")]
        public async Task<ActionResult> UpdateOrderStatus(OrderDTO order)
        {
            var response = await orderService.UpdateOrderStatus(order);

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
