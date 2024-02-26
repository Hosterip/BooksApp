using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Books.Commands.AddRemoveLike;

public class AddRemoveLikeCommandValidator : AbstractValidator<AddRemoveLikeCommand>
{
    public AddRemoveLikeCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(like => like.PostId)
            .MustAsync(async (bookId, cancellationToken) =>
            {
                return await unitOfWork.Books.AnyAsync(post => post.Id == bookId);
            })
            .WithMessage(ConstantsBookException.NotFound);
        RuleFor(like => like.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == userId);
            })
            .WithMessage(ConstantsUserException.NotFound);
    }
}