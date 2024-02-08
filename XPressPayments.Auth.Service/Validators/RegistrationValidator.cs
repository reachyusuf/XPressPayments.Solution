//using FluentValidation;
//using XPressPayments.Common.Dtos.AuthService;

//namespace XPressPayments.Auth.Service.Validators
//{
//    public class RegistrationValidator : AbstractValidator<RegisterDto>
//    {
//        public RegistrationValidator()
//        {
//            RuleFor(x => x.Email).NotEmpty().WithMessage("Sorry {PropertyName} required");
//            RuleFor(x => x.ProfileName).NotEmpty().WithMessage("Sorry {PropertyName} required");
//            RuleFor(x => x.Password).NotEmpty().WithMessage("Sorry {PropertyName} required");
//        }
//    }
//}
