using FluentValidation;

namespace PostsApp.Application.Auth.Queries.Login;

public sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().Length(10, 255);
        RuleFor(user => user.Password)
            .NotEmpty();
    }
}