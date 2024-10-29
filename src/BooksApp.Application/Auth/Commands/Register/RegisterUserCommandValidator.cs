using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Enums.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Auth.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .Must(email => new EmailAddressAttribute().IsValid(email));

        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
                !await unitOfWork.Users.AnyByEmail(email)).WithMessage(AuthValidationMessages.Occupied);

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, (int)UserMaxLengths.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength((int)UserMaxLengths.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength((int)UserMaxLengths.LastName);

        RuleFor(user => user.Password)
            .MaximumLength((int)UserMaxLengths.Password);

        RuleFor(user => user.Password)
            .NotEmpty();
    }
}