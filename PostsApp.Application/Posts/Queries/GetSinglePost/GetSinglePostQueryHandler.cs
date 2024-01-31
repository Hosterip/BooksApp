using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Posts.Queries.GetSinglePost;

internal sealed class GetSinglePostQueryHandler : IRequestHandler<GetSinglePostQuery, PostResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSinglePostQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PostResult> Handle(GetSinglePostQuery request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.Posts.GetSingleWhereAsync(post => post.Id == request.Id);

        var user = new UserResult { Id = post!.User.Id, Username = post!.User.Username };
        var likeCount = await 
            _unitOfWork.Likes.GetAllWhereAsync(like => like.User.Id == user.Id && like.Post.Id == request.Id);
        
        return new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user, LikeCount = likeCount.Count()};
    }
}