using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator(IAppDbContext dbContext)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Body).NotEmpty().Length(1, 255);
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                return await dbContext.Posts
                    .Include(post => post.User)
                    .AnyAsync(post => post.Id == request.Id && post.User.Id == request.UserId,
                    cancellationToken);
            }).WithMessage("Post must be yours");
    }
}