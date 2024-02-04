using XPressPayments.Common.Helpers;

namespace XPressPayments.Common.Dtos.AuthService
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string ProfileName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = Enums.UserRole.User.ToString();
    }
}
