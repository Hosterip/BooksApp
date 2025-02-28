using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using Mapster;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IUserService userService)
    : IRequestHandler<GetSingleUserQuery, UserResult>
{
    public async Task<UserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(
            request.Id, cancellationToken, includeRelationships: true);

        (bool IsFollowing, bool IsFriend, bool IsMe) viewerRelationship = (false, false, false);
        var currentUserId = userService.GetId();
        if (currentUserId.HasValue) 
            viewerRelationship = user!.ViewerRelationship(currentUserId);
        
        var userResult = mapper.Adapt<UserResult>();
        userResult.ViewerRelationship = new ViewerRelationship
        {
            IsFollowing = viewerRelationship.IsFollowing,
            IsFriend = viewerRelationship.IsFriend,
            IsMe = viewerRelationship.IsMe 
        };
        
        return userResult;
    }
}