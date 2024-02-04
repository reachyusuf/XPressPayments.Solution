using Microsoft.AspNetCore.Mvc;
using XPressPayments.Common.Dtos;
using static XPressPayments.Common.Helpers.Enums;

namespace XPressPayments.Core.Api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BaseResponseController : ControllerBase
    {
        public BaseResponseController()
        {

        }

        protected IActionResult TransformResponseWithHttpStatus<T>(OperationResult<T> result)
        {
            switch (result.Status)
            {
                case OperationResults.Successful:
                case OperationResults.Failed:
                case OperationResults.NotFound:
                    return Ok(result);
                case OperationResults.Unauthorized:
                    return Unauthorized(result);
                default:
                    return BadRequest(result);
            }
        }
  
        protected IActionResult ReturnValidationError<T>(string message)
        {
            OperationResult<T> result = new OperationResult<T>() { Message = message, Result = default, Status = OperationResults.BadRequest, Errors = new List<string>() { message } };
            return BadRequest(result);
        }

        protected IActionResult ReturnSuccess(dynamic result, string message = null, List<string> errors = null)
        {
            return Ok(new OperationResult<dynamic>() { Result = result, Status = OperationResults.Successful, Message = message ?? OperationResults.Successful.ToString() });
        }

        protected IActionResult ReturnBadRequest(string message = null, List<string> errors = null)
        {
            return BadRequest(new OperationResult<dynamic>() { Status = OperationResults.BadRequest, Message = message ?? OperationResults.BadRequest.ToString(), Errors = errors ?? new List<string>() { message ?? OperationResults.BadRequest.ToString() } });
        }

    }
}
