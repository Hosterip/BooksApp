using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Common.Helpers;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

public sealed class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator(
        IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) => !await unitOfWork.Users.AnyByEmail(email, cancellationToken))
            .WithMessage(ValidationMessages.User.Occupied);
        RuleFor(user => user.Email)
            .Must(EmailValidator.Validate)
            .WithMessage(ValidationMessages.User.InappropriateEmail);

        RuleFor(user => user.Email)
            .NotEmpty()
            .Length(1, MaxPropertyLength.User.Email);
    }
}