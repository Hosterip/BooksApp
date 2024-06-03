using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Commands.UpdateUser;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{

    public UpdateUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == id);
            })
            .WithMessage(ConstantsUserException.NotFound);
        RuleFor(user => user.NewUsername)
            .Length(0, 255)
            .NotEmpty();
        RuleFor(user => user.NewUsername)
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await unitOfWork.Users.AnyAsync(user => user.Username == username);
            })
            .WithMessage(ConstantsUserException.Occupied);
    }
}