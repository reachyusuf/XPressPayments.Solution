using Microsoft.AspNetCore.Mvc;
using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;
using XPressPayments.Common.Dtos.AuthService;

namespace XPressPayments.Core.Api.Controllers
{
    public class AuthController : BaseResponseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;
        }

        [ProducesResponseType(typeof(OperationResult<LoginResponseDto>), 200)]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            return TransformResponseWithHttpStatus(await _authService.Register(model));
        }

        [ProducesResponseType(typeof(OperationResult<LoginResponseDto>), 200)]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            return TransformResponseWithHttpStatus(await _authService.Login(model));
        }
    }

}
