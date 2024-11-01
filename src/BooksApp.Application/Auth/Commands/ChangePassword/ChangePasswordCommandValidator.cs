using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Enums.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.NewPassword)
            .MaximumLength((int)UserMaxLengths.Password);
    }
}