using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Common.Helpers;
using FluentValidation;

namespace BooksApp.Application.Auth.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .Must(EmailValidator.Validate)
            .WithMessage(UserValidationMessages.InappropriateEmail);

        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
                !await unitOfWork.Users.AnyByEmail(email, cancellationToken))
            .WithMessage(AuthValidationMessages.Occupied);

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, MaxPropertyLength.User.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength(MaxPropertyLength.User.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength(MaxPropertyLength.User.LastName);

        RuleFor(user => user.Password)
            .MaximumLength(MaxPropertyLength.User.Password);

        RuleFor(user => user.Password)
            .NotEmpty();
    }
}