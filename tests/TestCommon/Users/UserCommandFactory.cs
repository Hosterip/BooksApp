using BooksApp.Application.Users.Commands.AddRemoveFollower;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using Microsoft.AspNetCore.Http;
using TestCommon.Common.Constants;
using TestCommon.Images;

namespace TestCommon.Users;

public static class UserCommandFactory
{
    public static AddRemoveFollowerCommand CreateAddRemoveFollowerCommand(
        Guid? userId = null)
    {
        return new AddRemoveFollowerCommand
        {
            UserId = userId ?? Guid.NewGuid()
        };
    }
    public static DeleteUserCommand CreateDeleteUserCommand(
        Guid? userId = null)
    {
        return new DeleteUserCommand
        {
            UserId = userId ?? Guid.NewGuid()
        };
    }
    
    public static InsertAvatarCommand CreateInsertAvatarCommand(
        IFormFile? image = null)
    {
        return new InsertAvatarCommand
        {
            Image = image ?? ImageFactory.CreateFormFileImage()
        };
    }
    
    public static UpdateEmailCommand CreateUpdateEmailCommand(
        string email = Constants.Users.Email)
    {
        return new UpdateEmailCommand
        {
            Email = email
        };
    }
    public static UpdateNameCommand CreateUpdateNameCommand(
        string firstName = Constants.Users.FirstName,
        string? middleName = null,
        string? lastName = null)
    {
        return new UpdateNameCommand
        {
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName
        };
    }
}