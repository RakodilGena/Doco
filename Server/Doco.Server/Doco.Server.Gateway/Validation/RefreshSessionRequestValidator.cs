using Doco.Server.Gateway.Models.Requests.Auth;
using FluentValidation;

namespace Doco.Server.Gateway.Validation;

internal sealed class RefreshSessionRequestValidator : AbstractValidator<RefreshSessionRequest?>
{
    public RefreshSessionRequestValidator()
    {
        RuleFor(x => x).NotNull()
            .WithMessage("request object is invalid")
            .DependentRules(() =>
            {
                RuleFor(xx => xx!.UserId)
                    .NotEmpty()
                    .WithMessage("user id is required");

                RuleFor(xx => xx!.RefreshToken)
                    .NotEmpty()
                    .WithMessage("refresh token is required");
            });
    }
}