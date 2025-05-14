using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Auth.Queries.Login;

public sealed class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .Length(0, MaxPropertyLength.User.Email);
        RuleFor(user => user.Password)
            .NotEmpty();
        RuleFor(request => request.Email)
            .MustAsync(async (email, cancellationToken) =>
                await unitOfWork.Users.AnyByEmail(email, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
    }
}