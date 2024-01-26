using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
{

    public UpdateUsernameCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await unitOfWork.User.AnyAsync(user => user.Id == id);
            })
            .WithMessage("User not found");
        RuleFor(user => user.NewUsername)
            .Length(0, 255)
            .NotEmpty();
        RuleFor(user => user.NewUsername)
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await unitOfWork.User.AnyAsync(user => user.Username == username);
            }).WithMessage("New username is occupied");
    }
}