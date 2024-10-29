using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Auth.Commands.ChangePassword;
using BooksApp.Application.Auth.Commands.Register;
using BooksApp.Application.Auth.Queries.Login;
using BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using BooksApp.Contracts.Requests.Auth;
using BooksApp.Contracts.Responses.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Register(
        [FromBodyOrDefault] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Password = request.Password
        };
        var user = await _sender.Send(command, cancellationToken);
        var createDefaultBookshelves = new CreateDefaultBookshelvesCommand
        {
            UserId = Guid.Parse(user.Id)
        };
        await _sender.Send(createDefaultBookshelves, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Ok(user.Adapt<UserResponse>());
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login(
        [FromBodyOrDefault] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserQuery { Email = request.Email, Password = request.Password };
        var user = await _sender.Send(command, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Ok(user.Adapt<UserResponse>());
    }

    [HttpPost(ApiRoutes.Auth.UpdatePassword)]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> UpdatePassword(
        [FromBodyOrDefault] UpdatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand
        {
            NewPassword = request.NewPassword,
            OldPassword = request.OldPassword,
            Id = new Guid(HttpContext.GetId()!)
        };
        var result = await _sender.Send(command, cancellationToken);
        HttpContext.ChangeSecurityStamp(result.SecurityStamp);
        return Ok();
    }

    [HttpPost(ApiRoutes.Auth.Logout)]
    [Authorize(Policy = Policies.NotAuthorized)]
    public IActionResult Logout(
        HttpContext httpContext)
    {
        httpContext.SignOutAsync();
        return Ok();
    }
}