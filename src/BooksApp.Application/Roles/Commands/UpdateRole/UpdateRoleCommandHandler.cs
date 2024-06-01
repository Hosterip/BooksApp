using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Roles.Commands.UpdateRole; 

internal sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<UserResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
        var role = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == request.Role);
        user!.Role = role!;
        await _unitOfWork.SaveAsync(cancellationToken);
        return new UserResult
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role.Name,
            AvatarName = user.Avatar?.ImageName
        };
    }
}