using XPressPayments.Common.Dtos;
using XPressPayments.Common.Dtos.AuthService;

namespace XPressPayments.Business.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<bool>> Register(RegisterDto model);
        Task<OperationResult<LoginResponseDto>> Login(LoginDto model);
    }
}
