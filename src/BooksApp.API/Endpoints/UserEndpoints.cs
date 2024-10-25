using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Application.Images.Commands.CreateImage;
using PostsApp.Application.Images.Commands.DeleteImage;
using PostsApp.Application.Roles.Commands.UpdateRole;
using PostsApp.Application.Roles.Queries.GetRoles;
using PostsApp.Application.Users.Commands.DeleteUser;
using PostsApp.Application.Users.Commands.InsertAvatar;
using PostsApp.Application.Users.Commands.UpdateEmail;
using PostsApp.Application.Users.Commands.UpdateName;
using PostsApp.Application.Users.Queries.GetSingleUser;
using PostsApp.Application.Users.Queries.GetUsers;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Role;
using PostsApp.Common.Contracts.Requests.User;
using PostsApp.Common.Extensions;

namespace PostsApp.Controllers;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Users.GetMe, GetMe);
        app.MapGet(ApiEndpoints.Users.GetMany, GetMany);
        app.MapGet(ApiEndpoints.Users.GetById, GetById);

        app.MapDelete(ApiEndpoints.Users.Delete, Delete)
            .RequireAuthorization(Policies.Authorized);

        app.MapPut(ApiEndpoints.Users.UpdateEmail, UpdateEmail)
            .RequireAuthorization(Policies.Authorized);
        app.MapPut(ApiEndpoints.Users.UpdateName, UpdateName)
            .RequireAuthorization(Policies.Authorized);
        app.MapPut(ApiEndpoints.Users.UpdateAvatar, UpdateAvatar)
            .DisableAntiforgery()
            .RequireAuthorization(Policies.Authorized);
        // Roles 
        
        app.MapGet(ApiEndpoints.Users.GetAll, GetAll);

        app.MapPut(ApiEndpoints.Users.UpdateRole, UpdateRole)
            .RequireAuthorization(Policies.Authorized);
        
        // Bookshelves
        
        app.MapGet(ApiEndpoints.Users.GetBookshelves, GetBookshelves);
        
        // Books 
        
        app.MapGet(ApiEndpoints.Users.GetManyBooks, GetManyBooks);
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
        UpdateEmail request,
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
        UpdateName request,
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
        [FromForm]InsertAvatarRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        
        var command = new InsertAvatarCommand
        {
            Id = new Guid(httpContext.GetId()!),
            Image = request.Image
        };

        var result = await sender.Send(command, cancellationToken);

        return Results.Ok(result);
    }
    
    // Roles Logic

    public static async Task<IResult> GetAll(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new GetRoleQuery();

        var roles = await sender.Send(command, cancellationToken);

        return Results.Ok(roles);
    }
    
    public static async Task<IResult> UpdateRole(
        ChangeRoleRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand
        {
            ChangerId = new Guid(httpContext.GetId()!), 
            Role = request.Role, 
            UserId = request.UserId
        };

        await sender.Send(command, cancellationToken);
        
        return Results.Ok("Operation succeeded");
    }
    
    // Bookshelves 
    
    public static async Task<IResult> GetBookshelves(
        Guid userId,
        ISender sender)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await sender.Send(query);

        return Results.Ok(result);
    }
    
    // Books 
    
    public static async Task<IResult> GetManyBooks(
        CancellationToken cancellationToken,
        ISender sender,
        [FromRoute] Guid userId,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? genreId
    )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId, GenreId = genreId};
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}