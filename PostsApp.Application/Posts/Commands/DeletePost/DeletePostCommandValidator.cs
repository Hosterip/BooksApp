using FluentValidation;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Posts.Commands.DeletePost;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                return await unitOfWork.Post.AnyAsync(post => post.Id == request.Id && post.User.Id == request.UserId);
            }).WithMessage(PostExceptionConstants.PostNotYour);
    }
}