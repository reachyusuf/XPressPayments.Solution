using Microsoft.AspNetCore.Identity;
using static XPressPayments.Common.Helpers.Enums;

namespace XPressPayments.Data.Entities
{
    public class UserInfo : IdentityUser
    {
        public UserInfo()
        {

        }

        public UserInfo(string profileName, string email)
        {
            this.ProfileName = profileName;
            this.Email = email;
            this.UserName = email;
        }

        public string ProfileName { get; set; } = string.Empty;
        public string Role { get; set; } = UserRole.User.ToString();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}
