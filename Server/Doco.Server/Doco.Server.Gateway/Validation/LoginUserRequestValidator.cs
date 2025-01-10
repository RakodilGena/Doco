using Doco.Server.Gateway.Models.Requests.Auth;
using FluentValidation;

namespace Doco.Server.Gateway.Validation;

internal sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest?>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x).NotNull()
            .WithMessage("request object is invalid")
            .DependentRules(() =>
            {
                RuleFor(xx => xx!.Email)
                    .NotEmpty()
                    .WithMessage("email is required");

                RuleFor(xx => xx!.Password)
                    .NotEmpty()
                    .WithMessage("password is required");
            });
    }
}