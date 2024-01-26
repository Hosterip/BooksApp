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
        var post = await _unitOfWork.Post.GetSingleWhereAsync(post => post.Id == request.Id);

        var user = new UserResult { Username = post!.User.Username };

        return new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user };
    }
}