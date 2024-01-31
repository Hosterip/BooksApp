using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Posts.Commands.UpdatePost;

internal sealed class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePostCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PostResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.Posts.GetSingleWhereAsync(post => post.Id == request.Id);

        post!.Title = request.Title;
        post.Body = request.Body;

        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new PostResult
        {
            Id = post.Id, Title = post.Title, Body = post.Body, User = new UserResult{Username = post.User.Username, Id = post.User.Id}
        };
    }
}