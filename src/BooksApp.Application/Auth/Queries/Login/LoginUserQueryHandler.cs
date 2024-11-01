using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Queries.Login;

internal sealed class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await
            _unitOfWork.Users.GetSingleWhereAsync(user => user.Email == request.Email);

        if (!user!.IsPasswordValid(_passwordHasher, request.Password))
        {
            var property = $"{nameof(LoginUserQuery.Email)} and/or {nameof(LoginUserQuery.Password)}";
            throw new ValidationException([
                new ValidationFailure(property, AuthValidationMessages.EmailOrPassword)
            ]);
        }

        return _mapper.Map<AuthResult>(user!);
    }
}