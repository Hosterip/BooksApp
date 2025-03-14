using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Common.Helpers;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

public sealed class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) => 
                await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
        
        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await unitOfWork.Users.AnyAsync(
                           user => user.Email == email, cancellationToken);
            })
            .WithMessage(ValidationMessages.User.Occupied);
        RuleFor(user => user.Email)
            .Must(EmailValidator.Validate)
            .WithMessage(ValidationMessages.User.InappropriateEmail);
        
        RuleFor(user => user.Email)
            .NotEmpty()
            .Length(1, MaxPropertyLength.User.Email);
    }
}