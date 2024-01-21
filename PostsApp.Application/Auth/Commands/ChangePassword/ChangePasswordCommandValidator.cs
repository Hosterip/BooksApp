using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{

    public ChangePasswordCommandValidator(IAppDbContext dbContext)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await dbContext.Users.AnyAsync(user => user.Id == id, cancellationToken);
            })
            .WithMessage("Username must be correct");
    }
}