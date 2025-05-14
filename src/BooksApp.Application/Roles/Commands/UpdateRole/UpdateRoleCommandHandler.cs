using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Roles.Commands.UpdateRole;

internal sealed class UpdateRoleCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<UpdateRoleCommand, UserResult>
{
    public async Task<UserResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        var role = await unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == request.Role, cancellationToken);

        user!.ChangeRole(role!);

        await unitOfWork.Users.Update(user);
        await unitOfWork.SaveAsync(cancellationToken);
        return mapper.Map<UserResult>(user);
    }
}