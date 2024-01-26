using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Exceptions;

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
        var user = await _unitOfWork.User.GetSingleWhereAsync(user => user.Id == request.Id);
        if (user is null)
            throw new UserException(UserExceptionConstants.NotFound);

        var posts =
        (
            from post in await _unitOfWork.Post.GetAllWhereAsync(post => post.User.Id == user.Id)
            select new PostWithoutUser { Id = post.Id, Title = post.Title, Body = post.Body }
        ).ToArray();
        return new SingleUserResult { Id = user.Id, Username = user.Username, Posts = posts };
    }
}