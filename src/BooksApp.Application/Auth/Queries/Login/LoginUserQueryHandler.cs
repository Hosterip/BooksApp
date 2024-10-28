using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

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

        if (!Hashing.IsPasswordValid(user!.Hash, user.Salt, request.Password))
        {
            var property = $"{nameof(LoginUserQuery.Email)} and/or {nameof(LoginUserQuery.Password)}";
            throw new ValidationException([
                new ValidationFailure(property, AuthValidationMessages.EmailOrPassword)
            ]);
        }
        
        return _mapper.Map<AuthResult>(user!);
    }
}