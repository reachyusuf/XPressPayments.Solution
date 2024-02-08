using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XPressPayments.Common.Dtos;

namespace XPressPayments.Core.Api.Controllers.Admin
{
    [ProducesResponseType(typeof(OperationResult<IEnumerable<dynamic>>), 500)]
    [Route("api/admin/v1/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class BaseAdminController : BaseResponseController
    {

    }



   
}
