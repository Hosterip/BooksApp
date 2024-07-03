using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{

    public ChangePasswordCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(request.Id);
                if (user is null)
                    return false;
                return Hashing.IsPasswordValid(user!.Hash, user.Salt, request.OldPassword);
            })
            .WithMessage(AuthValidationMessages.UsernameOrPassword)
            .OverridePropertyName("OldPassword or Id");
    }
}