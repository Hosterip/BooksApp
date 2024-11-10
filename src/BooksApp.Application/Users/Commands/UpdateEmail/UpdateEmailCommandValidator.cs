using System.ComponentModel.DataAnnotations;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Enums.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.UpdateEmail;

public sealed class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) => 
                await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await unitOfWork.Users.AnyAsync(
                           user => user.Email == email, cancellationToken)
                       && new EmailAddressAttribute().IsValid(email);
            })
            .WithMessage(UserValidationMessages.Occupied);

        RuleFor(user => user.Email)
            .NotEmpty()
            .Length(1, (int)UserMaxLengths.Email);
    }
}