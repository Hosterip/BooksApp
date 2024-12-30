using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Auth;
using BooksApp.Application.Auth.Commands.ChangePassword;
using BooksApp.Application.Auth.Commands.Register;
using BooksApp.Application.Auth.Queries.Login;
using BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;
using BooksApp.Contracts.Auth;
using BooksApp.Contracts.Errors;
using BooksApp.Contracts.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class AuthController(ISender sender) : ApiController
{
    #region Authorization endpoints
    
    [HttpPost(ApiRoutes.Auth.Register)]
    [Authorize(Policies.NotAuthorized)]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Register(
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
        var user = await sender.Send(command, cancellationToken);
        var createDefaultBookshelves = new CreateDefaultBookshelvesCommand
        {
            UserId = Guid.Parse(user.Id)
        };
        await sender.Send(createDefaultBookshelves, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return CreatedAtAction(
            nameof(UsersController.GetById),
            "Users",
            new { userId = user.Id },
            user.Adapt<UserResponse>());
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    [Authorize(Policies.NotAuthorized)]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Login(
        [FromBodyOrDefault] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserQuery { Email = request.Email, Password = request.Password };
        var user = await sender.Send(command, cancellationToken);
        await HttpContext.Login(user.Id, user.Email, user.Role, user.SecurityStamp);
        return Ok(user.Adapt<UserResponse>());
    }
    
    [HttpPost(ApiRoutes.Auth.Logout)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return NoContent();
    }
    
    #endregion Authorization endpoints
    
    [HttpPut(ApiRoutes.Auth.UpdatePassword)]
    [Authorize]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> UpdatePassword(
        [FromBodyOrDefault] UpdatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand
        {
            NewPassword = request.NewPassword,
            OldPassword = request.OldPassword,
            Id = HttpContext.GetId()!.Value
        };
        var result = await sender.Send(command, cancellationToken);
        HttpContext.ChangeSecurityStamp(result.SecurityStamp);
        return CreatedAtAction(
            nameof(UsersController.GetById),
            "Users",
            new { id = result.Id },
            result.Adapt<UserResponse>());
    }
}