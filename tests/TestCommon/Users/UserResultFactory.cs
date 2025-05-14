using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Role;
using TestCommon.Common.Constants;

namespace TestCommon.Users;

public static class UserResultFactory
{
    public static UserResult CreateUserResult(
        Guid? id = null,
        string email = Constants.Users.Email,
        string firstName = Constants.Users.FirstName,
        string? middleName = null,
        string? lastName = null,
        string? role = null,
        string? avatar = null,
        ViewerRelationship? viewerRelationship = null)
    {
        return new UserResult
        {
            Id = id.ToString() ?? Guid.NewGuid().ToString(),
            Email = email,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Role = role ?? RoleFactory.Member().Name,
            AvatarName = avatar ?? Constants.Images.ImageName,
            ViewerRelationship = viewerRelationship
        };
    }
}