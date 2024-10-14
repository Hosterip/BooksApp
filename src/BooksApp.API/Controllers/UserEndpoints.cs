using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Images.Commands.CreateImage;
using PostsApp.Application.Images.Commands.DeleteImage;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.InsertAvatar;
using PostsApp.Application.Users.Commands.UpdateEmail;
using PostsApp.Application.Users.Commands.UpdateName;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("api/users");

        users.MapGet("me", GetMe);
        users.MapGet("", GetMany);
        users.MapGet("{id:guid}", GetById);

        users.MapDelete("", Delete)
            .RequireAuthorization(Policies.Authorized);

        users.MapPut("email", UpdateEmail)
            .RequireAuthorization(Policies.Authorized);
        users.MapPut("name", UpdateName)
            .RequireAuthorization(Policies.Authorized);
        users.MapPut("avatar", UpdateAvatar)
            .RequireAuthorization(Policies.Authorized);
    }

    public static async Task<IResult> GetMe(
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var id = new Guid(httpContext.GetId()!);
        var query = new GetSingleUserQuery { Id = id };
        var user = await sender.Send(query, cancellationToken);
        return Results.Ok(user);
    }

    public static async Task<IResult> GetMany(
        int? page,
        int? limit,
        string? q,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery { Query = q, Page = page, Limit = limit };
        var users = await sender.Send(query, cancellationToken);
        return Results.Ok(users);
    }

    public static async Task<IResult> GetById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleUserQuery { Id = id };
        var user = await sender.Send(query, cancellationToken);
        return Results.Ok(user);
    }

    public static async Task<IResult> Delete(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand { Id = new Guid(httpContext.GetId()!) };
        await sender.Send(command, cancellationToken);

        await httpContext.SignOutAsync();

        return Results.Ok("User was deleted");
    }

    public static async Task<IResult> UpdateEmail(
        [FromBodyOrDefault] UpdateEmail request,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmailCommand
        {
            Id = new Guid(httpContext.GetId()!),
            Email = request.Email,
        };

        await sender.Send(command, cancellationToken);

        httpContext.ChangeEmail(request.Email);

        return Results.Ok("Email was updated");
    }

    public static async Task<IResult> UpdateName(
        [FromBodyOrDefault] UpdateName request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNameCommand
        {
            UserId = new Guid(httpContext.GetId()!),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
        };

        await sender.Send(command, cancellationToken);

        return Results.Ok("Name was updated");
    }
    
    public static async Task<IResult> UpdateAvatar(
        [FromBodyOrDefault] InsertAvatarRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var createImageCommand = new CreateImageCommand
        {
            Image = request.Image
        };

        var imageName = await sender.Send(createImageCommand, cancellationToken);

        var userQuery = new GetSingleUserQuery
        {
            Id = new Guid(httpContext.GetId()!)
        };

        var user = await sender.Send(userQuery, cancellationToken);

        if (user.AvatarName is not null)
        {
            var deleteImageCommand = new DeleteImageCommand
            {
                ImageName = user.AvatarName
            };
            await sender.Send(deleteImageCommand, cancellationToken);
        }

        var command = new InsertAvatarCommand
        {
            Id = new Guid(httpContext.GetId()!),
            ImageName = imageName
        };

        var result = await sender.Send(command, cancellationToken);

        return Results.Ok(result);
    }
}