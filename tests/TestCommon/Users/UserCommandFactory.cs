using Bogus;
using BooksApp.Application.Users.Commands.AddRemoveFollower;
using BooksApp.Application.Users.Commands.DeleteUser;
using BooksApp.Application.Users.Commands.InsertAvatar;
using BooksApp.Application.Users.Commands.UpdateEmail;
using BooksApp.Application.Users.Commands.UpdateName;
using Microsoft.AspNetCore.Http;
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
        string? email = null)
    {
        return new Faker<UpdateEmailCommand>()
            .RuleFor(x => x.Email, f => email ?? f.Person.Email);
    }

    public static UpdateNameCommand CreateUpdateNameCommand(
        string? firstName = null,
        string? middleName = null,
        string? lastName = null)
    {
        return new Faker<UpdateNameCommand>()
            .RuleFor(x => x.FirstName, f => firstName ?? f.Person.FirstName)
            .RuleFor(x => x.MiddleName, _ => middleName)
            .RuleFor(x => x.LastName, _ => lastName);
    }
}