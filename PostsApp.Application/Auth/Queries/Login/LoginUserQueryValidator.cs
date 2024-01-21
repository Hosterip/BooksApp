using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator(IAppDbContext dbContext)
    {
        RuleFor(user => user.Username)
            .NotEmpty().Length(0, 255);
        RuleFor(user => user.Username)
            .MustAsync(async (username, cancellationToken) =>
            {
                return await dbContext.Users.AnyAsync(user => user.Username == username, cancellationToken);
            }).WithMessage("User not found");
        RuleFor(user => user.Password)
            .NotEmpty();
    }
}