using FluentValidation;

namespace PostsApp.Application.Auth.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().Length(0, 255);
        RuleFor(user => user.Password)
            .NotEmpty();
    }
}