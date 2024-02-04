using System.ComponentModel;

namespace XPressPayments.Common.Helpers
{
    public class Enums
    {
        public enum UserRole
        {
            [Description("Admin")]
            Admin = 1,
            [Description("User")]
            User = 2
        }

        public enum OperationResults
        {
            [Description("Successful")]
            Successful = 1,
            [Description("InternalServerError")]
            InternalServerError = 0,
            [Description("Validation")]
            Validation = -2,
            [Description("NotFound")]
            NotFound = -3,
            [Description("Failed")]
            Failed = -4,
            [Description("Unauthorized")]
            Unauthorized = -7,
            [Description("BadRequest")]
            BadRequest = -8,
            [Description("Forbidden")]
            Forbidden = -9,
            [Description("UnavailableService")]
            UnavailableService = -11,
            [Description("UnVerifiedUser")]
            UnVerifiedUser = -16
        }

    }
}
