using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Books.Commands.AddRemoveLike;

public class AddRemoveLikeCommandHandler : IRequestHandler<AddRemoveLikeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddRemoveLikeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(AddRemoveLikeCommand request, CancellationToken cancellationToken)
    {
        if (
            await _unitOfWork.Likes
                .AnyAsync(like => like.User.Id == request.UserId && like.Book.Id == request.PostId))
        {
            var like = await _unitOfWork.Likes.GetSingleWhereAsync(like =>
                like.User.Id == request.UserId && like.Book.Id == request.PostId);
            _unitOfWork.Likes.Remove(like!);
            await _unitOfWork.SaveAsync(cancellationToken);
            return;
        }

        var post = await _unitOfWork.Books.GetSingleWhereAsync(post => post.Id == request.PostId);
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
        var newLike = new Like{User = user!, Book = post!};
        await _unitOfWork.Likes.AddAsync(newLike);
    }
}