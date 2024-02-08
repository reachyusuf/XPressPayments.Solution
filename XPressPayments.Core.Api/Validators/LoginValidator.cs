using FluentValidation;
using XPressPayments.Common.Dtos.AuthService;

namespace XPressPayments.Core.Api.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Sorry {PropertyName} required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Sorry {PropertyName} required");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Sorry {PropertyName} required");
        }
    }
}
