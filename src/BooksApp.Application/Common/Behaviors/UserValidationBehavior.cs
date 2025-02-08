using BooksApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BooksApp.Application.Common.Behaviors;

public class UserValidationBehavior<TRequest, TResponse>(
    IHttpContextAccessor accessor,
    IUserService userService,
    IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var id = userService.GetId();
        var securityStamp = userService.GetSecurityStamp();
        var role = userService.GetRole();
        if (id != null && securityStamp != null)
        {
            var user = await unitOfWork.Users.GetSingleById(id.Value, cancellationToken);
            if (user != null && user.SecurityStamp == securityStamp)
                userService.ChangeRole(user.Role.Name);
            else
                await accessor.HttpContext!.SignOutAsync();
        }

        return await next();
    }
}