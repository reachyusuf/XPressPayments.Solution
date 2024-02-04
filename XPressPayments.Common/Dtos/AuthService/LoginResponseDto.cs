namespace XPressPayments.Common.Dtos.AuthService
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string ProfileName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
