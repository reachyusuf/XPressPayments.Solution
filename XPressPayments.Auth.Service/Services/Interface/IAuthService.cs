namespace XPressPayments.Auth.Service.Services.Interface
{
    public interface IAuthService
    {
        //Task<OperationResult<bool>> Register(RegisterDto model);
        //Task<OperationResult<LoginResponseDto>> Login(LoginDto model);

        Task<dynamic> Register(dynamic model);
        Task<dynamic> Login(dynamic model);
    }
}
