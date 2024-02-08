using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XPressPayments.Business.Interfaces;
using XPressPayments.Common.Dtos;
using XPressPayments.Common.Dtos.AuthService;
using XPressPayments.Data.Entities;

namespace XPressPayments.Business.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly AppSettingsDto _appSettings = null;


        public AuthService(UserManager<UserInfo> _userManager, SignInManager<UserInfo> _signInManager, IOptions<AppSettingsDto> appSettings)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._appSettings = appSettings?.Value ?? throw new Exception("Appsettings.json file is not found");
        }

        public async Task<OperationResult<bool>> Register(RegisterDto model)
        {
            var newUser = new UserInfo { ProfileName = model.ProfileName, UserName = model.Email, Email = model.Email, Role = model.Role, CreatedDate = DateTime.Now };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result?.Succeeded is false)
            {
                var errMsg = new List<string>();
                foreach (var item in result.Errors)
                {
                    errMsg.Add(item.Description);
                }
                return OperationResult<bool>.ReturnBadRequest(messages: errMsg);
            }

            return OperationResult<bool>.ReturnSuccess(true, "Registration successful");
        }

        public async Task<OperationResult<LoginResponseDto>> Login(LoginDto model)
        {
            LoginResponseDto response = null;
            var _authResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            if (_authResult?.IsLockedOut is true)
                return OperationResult<LoginResponseDto>.ReturnBadRequest("Sorry! account is locked due to repeated failed login attempts");
            else if (_authResult?.Succeeded is false)
                return OperationResult<LoginResponseDto>.ReturnBadRequest("Invalid Phone No or password");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return OperationResult<LoginResponseDto>.ReturnBadRequest("Sorry! user not found");

            response = await GenerateSecurityToken(user);
            return OperationResult<LoginResponseDto>.ReturnSuccess(response);
        }

        private async Task<LoginResponseDto> GenerateSecurityToken(UserInfo user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.ProfileName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.Now.AddMinutes(_appSettings.Jwt.JwtTokenExpiredTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.Jwt.JwtIssuer,
                Issuer = _appSettings.Jwt.JwtIssuer
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var UserToken = tokenHandler.WriteToken(securityToken);
            var authResponseModel = new LoginResponseDto() { Email = user.Email, ProfileName = user.ProfileName, Role = user.Role, Token = UserToken, UserId = user.Id };
            return authResponseModel;
        }
    }

}
