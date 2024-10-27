using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Roles.Commands.UpdateRole;

internal sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UserResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var role = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == request.Role);
        user!.Role = role!;
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<UserResult>(user);
    }
}