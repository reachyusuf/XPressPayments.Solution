using Microsoft.AspNetCore.Mvc;
using XPressPayments.Core.Api.Filters;
using XPressPayments.Core.Api.Services.Interface;

namespace XPressPayments.Core.Api.Controllers
{
    [JobsAuthFilter]
    public class JobsController : BaseResponseController
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService _jobService)
        {
            this._jobService = _jobService;
        }

        [HttpGet]
        public async Task<IActionResult> SendDailyWelcomeEmail()
        {
            return TransformResponseWithHttpStatus(await _jobService.SendDailyWelcomeEmail());
        }

        [HttpGet]
        public async Task<IActionResult> SendDailyReport()
        {
            return TransformResponseWithHttpStatus(await _jobService.SendDailyReport());
        }
    }
}
