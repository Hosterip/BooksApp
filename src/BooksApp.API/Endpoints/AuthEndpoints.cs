using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using PostsApp.Application.Auth.Commands.ChangePassword;
using PostsApp.Application.Auth.Commands.Register;
using PostsApp.Application.Auth.Queries.Login;
using PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Auth;
using PostsApp.Common.Contracts.Responses.User;
using PostsApp.Common.Extensions;

namespace PostsApp.Controllers;

public static class AuthEndpoints 
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapPost(ApiEndpoints.Auth.Register, Register)
            .RequireAuthorization(Policies.NotAuthorized);
        app.MapPost(ApiEndpoints.Auth.Login, Login)
            .RequireAuthorization(Policies.NotAuthorized);
        app.MapPost(ApiEndpoints.Auth.Logout, Logout)
            .RequireAuthorization(Policies.Authorized);
        
        app.MapPut(ApiEndpoints.Auth.UpdatePassword, UpdatePassword)
            .RequireAuthorization(Policies.NotAuthorized);
    }
    
    public static async Task<IResult> Register(
        RegisterRequest request,
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
        LoginRequest request,
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
        AuthUpdatePasswordRequest request,
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

    public static IResult Logout(
        HttpContext httpContext)
    {
        httpContext.SignOutAsync();
        return Results.Ok("You've been signed out");
    }
}