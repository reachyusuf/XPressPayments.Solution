using Microsoft.AspNetCore.Mvc;
using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;

namespace XPressPayments.Core.Api.Controllers.Admin
{
    public class AdminController : BaseAdminController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [ProducesResponseType(typeof(OperationResult<bool>), 500)]
        [ProducesResponseType(typeof(OperationResult<bool>), 400)]
        [HttpGet]
        public async Task<IActionResult> List(int pageIndex = 1, string? search = null)
        {
            return this.TransformResponseWithHttpStatus(await _userService.List(pageIndex, search));
        }

        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [ProducesResponseType(typeof(OperationResult<bool>), 500)]
        [ProducesResponseType(typeof(OperationResult<bool>), 400)]
        [HttpGet]
        public async Task<IActionResult> ListByLinq(int pageIndex = 1, string? search = null)
        {
            return this.TransformResponseWithHttpStatus(await _userService.ListByLinq(pageIndex, search));
        }

        [ProducesResponseType(typeof(OperationResult<bool>), 200)]
        [HttpGet]
        public async Task<IActionResult> ListByStoredProc(int pageIndex = 1, string? search = null)
        {
            return this.TransformResponseWithHttpStatus(await _userService.ListByStoredProc(pageIndex, search));
        }

    }
}
