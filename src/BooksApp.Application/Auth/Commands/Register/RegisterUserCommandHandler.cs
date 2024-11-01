using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Interfaces;
using BooksApp.Domain.User;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var hashSalt = _passwordHasher.GenerateHashSalt(request.Password);
        var memberRole = await _unitOfWork.Roles.GetSingleWhereAsync(role => role.Name == RoleNames.Member);

        var user = User.Create(
            request.Email,
            request.FirstName,
            request.MiddleName,
            request.LastName,
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