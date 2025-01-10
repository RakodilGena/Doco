using Doco.Server.Gateway.Models.Requests.Users;
using FluentValidation;

namespace Doco.Server.Gateway.Validation;

internal sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest?>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x).NotNull()
            .WithMessage("request object is invalid")
            .DependentRules(() =>
            {
                RuleFor(xx => xx!.Email)
                    .NotEmpty()
                    .WithMessage("email is required")
                    .EmailAddress()
                    .WithMessage("email is invalid");

                RuleFor(xx => xx!.Name)
                    .NotEmpty()
                    .WithMessage("user name is required")
                    .Length(5, 30)
                    .WithMessage("user name must be between 5 and 30 characters");

                RuleFor(xx => xx!.Password)
                    .NotEmpty()
                    .WithMessage("password is required")
                    .Length(5, 100)
                    .WithMessage("password must be between 5 and 100 characters");
            });
    }
}