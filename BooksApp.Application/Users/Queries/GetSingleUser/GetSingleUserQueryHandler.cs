using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Exceptions;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, SingleUserResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SingleUserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.Id);
        if (user is null)
            throw new UserException(UserExceptionConstants.NotFound);

        var rawLikes = await
            _unitOfWork.Likes.GetAllWhereAsync(like => like.User.Id == request.Id);

        var likes = (
            from like in rawLikes
            select new LikeResult { Id = like.Id, UserId = like.User.Id, PostId = like.Book.Id }
        ).ToArray();

        var posts = await _unitOfWork.Posts.GetBooks(post => request.Id == post.Author.Id);
        return new SingleUserResult { Id = user.Id, Username = user.Username, Role = user.Role ?? Roles.Member, Posts = posts.ToArray(), Likes = likes };
    }
}