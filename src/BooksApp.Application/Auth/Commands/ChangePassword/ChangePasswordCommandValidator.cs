using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
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