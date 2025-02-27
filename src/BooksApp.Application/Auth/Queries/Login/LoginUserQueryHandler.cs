using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Errors;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using MapsterMapper;
using MediatR;
using ValidationException = BooksApp.Application.Common.Errors.ValidationException;

namespace BooksApp.Application.Auth.Queries.Login;

internal sealed class LoginUserQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginUserQuery, AuthResult>
{
    public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await
            unitOfWork.Users.GetSingleWhereAsync(
                user => user.Email == request.Email,
                cancellationToken);

        if (!user!.IsPasswordValid(passwordHasher, request.Password))
        {
            var property = $"{nameof(LoginUserQuery.Email)} and/or {nameof(LoginUserQuery.Password)}";
            throw new ValidationException([
                new ValidationFailure {
                    PropertyName = property,
                    ErrorMessage = ValidationMessages.Auth.EmailOrPassword
                }
            ]);
        }

        return mapper.Map<AuthResult>(user);
    }
}