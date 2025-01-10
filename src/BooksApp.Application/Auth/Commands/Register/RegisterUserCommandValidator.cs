using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .Must(email => new EmailAddressAttribute().IsValid(email));

        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
                !await unitOfWork.Users.AnyByEmail(email, cancellationToken))
            .WithMessage(AuthValidationMessages.Occupied);

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, UserMaxLengths.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength(UserMaxLengths.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength(UserMaxLengths.LastName);

        RuleFor(user => user.Password)
            .MaximumLength(UserMaxLengths.Password);

        RuleFor(user => user.Password)
            .NotEmpty();
    }
}