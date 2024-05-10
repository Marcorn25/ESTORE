using System.Net;

namespace ESTORE.Server.Services.ResponseServices
{
    public class Response
    {
        public record class GeneralResponse(string Message, HttpStatusCode ResponseCode = HttpStatusCode.OK);
        public record class LoginResponse(string Token, string Message, HttpStatusCode ResponseCode = HttpStatusCode.OK);
        public record DataResponse<T>(T Data, string Message, HttpStatusCode ResponseCode = HttpStatusCode.OK);
    }
}
