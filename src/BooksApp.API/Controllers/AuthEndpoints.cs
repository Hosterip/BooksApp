using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Auth;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public static class AuthEndpoints 
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("api/auth");

        auth.MapPost("register", Register)
            .RequireAuthorization(Policies.NotAuthorized);
        auth.MapPost("login", Login)
            .RequireAuthorization(Policies.NotAuthorized);
        auth.MapPost("logout", Logout)
            .RequireAuthorization(Policies.Authorized);
        
        auth.MapPut("updatePassword", UpdatePassword)
            .RequireAuthorization(Policies.NotAuthorized);
    }
    
    public static async Task<IResult> Register(
        [FromBodyOrDefault] RegisterRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Password = request.Password,
        };
        var user = await sender.Send(command, cancellationToken);
        var createDefaultBookshelves = new CreateDefaultBookshelvesCommand
        {
            UserId = Guid.Parse(user.Id)
        };
        await sender.Send(createDefaultBookshelves, cancellationToken);
        await httpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Results.Ok(user.Adapt<UserResponse>());
    }

    public static async Task<IResult> Login(
        [FromBodyOrDefault] LoginRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserQuery { Email = request.Email, Password = request.Password };
        var user = await sender.Send(command, cancellationToken);
        await httpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Results.Ok(user.Adapt<UserResponse>());
    }

    public static async Task<IResult> UpdatePassword(
        [FromBodyOrDefault] AuthUpdatePasswordRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand
        {
            NewPassword = request.NewPassword,
            OldPassword = request.OldPassword,
            Id = new Guid(httpContext.GetId()!)
        };
        var result = await sender.Send(command, cancellationToken);
        httpContext.ChangeSecurityStamp(result.SecurityStamp);
        return Results.Ok("Operation succeeded");
    }

    public static IResult Logout(HttpContext httpContext)
    {
        httpContext.SignOutAsync();
        return Results.Ok("You've been signed out");
    }
}