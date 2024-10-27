using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Auth.Queries.Login;

internal sealed class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await
            _unitOfWork.Users.GetSingleWhereAsync(user => user.Email == request.Email);

        return _mapper.Map<AuthResult>(user!);
    }
}