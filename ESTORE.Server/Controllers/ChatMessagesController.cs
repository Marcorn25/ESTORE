using ESTORE.Server.Services.ChatMessageServices;
using ESTORE.Shared.DTO.ChatMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESTORE.Server.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageService chatMessageService;

        public ChatMessagesController(IChatMessageService chatMessageService)
        {
            this.chatMessageService = chatMessageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChatMessageDTO>>> GetAllMessages()
        {
            var response = await chatMessageService.GetAllChatMessage();

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
    }
}
