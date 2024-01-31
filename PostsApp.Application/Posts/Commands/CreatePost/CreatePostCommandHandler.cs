using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Posts.Commands.CreatePost;

internal sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePostCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.Id);
        var post = new Post { User = user!, Title = request.Title, Body = request.Body };
        await _unitOfWork.Posts.AddAsync(post);
        await _unitOfWork.SaveAsync(cancellationToken);
        var result = new PostResult
        {
            User = new UserResult { Id = user!.Id, Username = user.Username }, Title = post.Title, Body = post.Body,
            Id = post.Id
        };
        return result;
    }
}