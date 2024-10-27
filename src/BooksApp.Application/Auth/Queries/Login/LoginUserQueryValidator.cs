using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Auth.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Email)
            .NotEmpty().Length(0, 255);
        RuleFor(user => user.Password)
            .NotEmpty();
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleWhereAsync(user => user.Email == request.Email);
                
                if (user is null) return false;
                
                return Hashing.IsPasswordValid(user.Hash, user.Salt, request.Password);
            })
            .WithMessage(AuthValidationMessages.EmailOrPassword)
            .OverridePropertyName($"{nameof(LoginUserQuery.Email)} and/or {nameof(LoginUserQuery.Password)}");
    }
}