using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .NotEmpty().Length(0, 255);
        RuleFor(user => user.Password)
            .NotEmpty();
        RuleFor(request => request.Email)
            .MustAsync(async (email, cancellationToken) => await unitOfWork.Users.AnyByEmail(email))
            .WithMessage(UserValidationMessages.NotFound);
    }
}