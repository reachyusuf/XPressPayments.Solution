using XPressPayments.Common.Extensions;
using static XPressPayments.Common.Helpers.Enums;

namespace XPressPayments.Common.Dtos
{
    public class OperationResult<T>
    {
        public OperationResults Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public List<string> Errors { get; set; }

        public static OperationResult<T> ReturnSuccess(T result, string message = null, List<string> messages = null, int totalRows = 0)
        {
            return new OperationResult<T>() { Status = OperationResults.Successful, Message = message ?? OperationResults.Successful.GetDisplayname(), Result = result };
        }

        public static OperationResult<T> ReturnBadRequest(string message = null, List<string> messages = null)
        {
            var result = new OperationResult<T>() { Status = OperationResults.BadRequest, Message = message ?? OperationResults.BadRequest.GetDisplayname(), Errors = messages ?? new List<string>() { message ?? OperationResults.BadRequest.GetDisplayname() } };

            if (string.IsNullOrEmpty(message) && messages is not null) result.Message = messages.FirstOrDefault();
            if (messages == null && !string.IsNullOrEmpty(message)) result.Errors = new List<string>() { message };
            return result;
        }

        public static OperationResult<T> ReturnFailedRequest(string message = null, List<string> messages = null)
        {
            var result = new OperationResult<T>() { Status = OperationResults.Failed, Message = message ?? OperationResults.Failed.GetDisplayname(), Errors = messages ?? new List<string>() { message ?? OperationResults.Failed.GetDisplayname() } };

            if (string.IsNullOrEmpty(message) && messages is not null) result.Message = messages.FirstOrDefault();
            if (messages == null && !string.IsNullOrEmpty(message)) result.Errors = new List<string>() { message };
            return result;
        }

        public static OperationResult<T> ReturnUnAuthorizedRequest(string message = null, List<string> messages = null)
        {
            var result = new OperationResult<T>() { Status = OperationResults.Unauthorized, Message = message ?? OperationResults.Unauthorized.GetDisplayname(), Errors = messages ?? new List<string>() { message ?? OperationResults.Unauthorized.GetDisplayname() } };

            if (string.IsNullOrEmpty(message) && messages is not null)
                result.Message = messages.FirstOrDefault();

            if (messages == null && !string.IsNullOrEmpty(message))
                result.Errors = new List<string>() { message };

            return result;
        }
        public static OperationResult<T> ReturnException(string message = null, List<string> messages = null)
        {
            var result = new OperationResult<T>() { Status = OperationResults.InternalServerError, Message = message ?? OperationResults.InternalServerError.GetDisplayname(), Errors = messages ?? new List<string>() { message ?? OperationResults.InternalServerError.GetDisplayname() } };

            if (string.IsNullOrEmpty(message) && messages is not null)
                result.Message = messages.FirstOrDefault();

            if (messages == null && !string.IsNullOrEmpty(message))
                result.Errors = new List<string>() { message };

            return result;
        }


    }

}
