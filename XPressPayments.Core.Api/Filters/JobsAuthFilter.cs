using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XPressPayments.Common.Dtos;

namespace XPressPayments.Core.Api.Filters
{
    public class JobsAuthFilterAttribute : Attribute, IAsyncActionFilter
    {
        private static string ApiKeyHeaderName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //--Before the executing an action
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            //--check if the passed apikey from the header is authorized 
            if (!await IsAuthorized(context, potentialKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

        public async Task<bool> IsAuthorized(ActionExecutingContext context, string apikey)
        {
            var flag = false;
            if (string.IsNullOrEmpty(apikey) is true) return flag;

            var _config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var clientKey = _config.GetSection("").Get<AppSettingsDto>();
            flag = (apikey.ToLower() == clientKey?.ClientKey?.ToLower());
            return flag;
        }
    }

}
