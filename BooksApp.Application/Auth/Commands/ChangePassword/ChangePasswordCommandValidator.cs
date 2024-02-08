using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{

    public ChangePasswordCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == id);
            })
            .WithMessage(AuthExceptionConstants.NotFound);
    }
}