using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
{

    public UpdateUsernameCommandValidator(IAppDbContext dbContext)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                var user = await dbContext.Users
                    .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
                return user != null;
            })
            .WithMessage("User not found");
        RuleFor(user => user.NewUsername)
            .Length(0, 255)
            .NotEmpty();
        RuleFor(user => user.NewUsername)
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await dbContext.Users.AnyAsync(user => user.Username == username, cancellationToken);
            }).WithMessage("New username is occupied");
    }
}