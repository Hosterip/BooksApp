using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Common.Constants;
using PostsApp.Domain.Common.Security;
using PostsApp.Domain.Role;
using PostsApp.Domain.User;

namespace PostsApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashSalt = Hashing.GenerateHashSalt(request.Password);
        var memberRole = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == RoleNames.Member);
        
        User user = User.Create(
            request.Email,
            request.FirstName, 
            null,
            null,
            memberRole!,
            hashSalt.Hash, 
            hashSalt.Salt,
            null
        );
        
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<AuthResult>(user!);
    }
}