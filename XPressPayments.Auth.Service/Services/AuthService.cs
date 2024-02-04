using XPressPayments.Auth.Service.Services.Interface;
using XPressPayments.Common.Dtos;
using XPressPayments.Common.Dtos.AuthService;

namespace XPressPayments.Auth.Service.Services
{
    public class AuthService : IAuthService
    {
        public async Task<OperationResult<LoginResponseDto>> Login(LoginDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<bool>> Register(RegisterDto model)
        {
            throw new NotImplementedException();
        }
    }
}
