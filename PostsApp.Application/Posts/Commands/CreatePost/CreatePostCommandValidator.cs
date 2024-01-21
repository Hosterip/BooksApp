using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator(IAppDbContext dbContext)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Body).NotEmpty().Length(1, 255);
        RuleFor(post => post.Id).MustAsync(async (id, cancellationToken) =>
        {
            var user = await 
                dbContext.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
            return user != null;
        }).WithMessage("User must be registered");
        
    }
}