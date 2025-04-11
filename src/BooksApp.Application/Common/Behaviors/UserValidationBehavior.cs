using System.Reflection;
using BooksApp.Application.Common.Attributes;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Errors;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Common.Behaviors;

public class UserValidationBehavior<TRequest, TResponse>(
    IUserService userService,
    IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attribute = request.GetType().GetCustomAttribute<AuthorizeAttribute>();
        if (attribute == null)
        {
            return await next();
        }
        
        var id = userService.GetId();
        var securityStamp = userService.GetSecurityStamp();
        var role = userService.GetRole();
        if (id != null && securityStamp != null)
        {
            var user = await unitOfWork.Users.GetSingleById(id.Value, cancellationToken);
            if (user != null &&
                (user.SecurityStamp == securityStamp || user.Role.Name != role))
                userService.ChangeRole(user.Role.Name);
            else if (user == null || user.SecurityStamp != securityStamp)
            {
                await userService.Logout();
                throw new ValidationException([
                    new ValidationFailure
                    {
                        PropertyName = nameof(UserId),
                        ErrorMessage = ValidationMessages.User.NotFound
                    }
                ]);
            }
        }

        return await next();
    }
}