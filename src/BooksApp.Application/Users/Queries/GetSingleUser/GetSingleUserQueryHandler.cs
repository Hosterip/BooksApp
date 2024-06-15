using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id.Value == request.Id);

        return new UserResult
        {
            Id = user!.Id.Value.ToString(),
            Username = user.Username,
            Role = user.Role.Name,
            AvatarName = user.Avatar?.ImageName
        };
    }
}