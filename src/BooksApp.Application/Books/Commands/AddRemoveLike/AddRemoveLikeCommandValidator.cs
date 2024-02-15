using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Constants.Exceptions;

namespace PostsApp.Application.Books.Commands.AddRemoveLike;

public class AddRemoveLikeCommandValidator : AbstractValidator<AddRemoveLikeCommand>
{
    public AddRemoveLikeCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(like => like)
            .MustAsync(async (like, cancellationToken) =>
            {
                return await unitOfWork.Posts.AnyAsync(post => post.Id == like.PostId);
            })
            .OverridePropertyName("Post Id")
            .WithMessage(BookExceptionConstants.NotFound);
        RuleFor(like => like)
            .MustAsync(async (like, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == like.UserId);
            })
            .OverridePropertyName("User Id")
            .WithMessage(UserExceptionConstants.NotFound);
    }
}