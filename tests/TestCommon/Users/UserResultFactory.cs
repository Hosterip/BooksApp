using Bogus;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Role;

namespace TestCommon.Users;

public static class UserResultFactory
{
    public static UserResult CreateUserResult(
        Guid? id = null,
        string? email = null,
        string? firstName = null,
        string? middleName = null,
        string? lastName = null,
        string? role = null,
        string? avatar = null,
        ViewerRelationship? viewerRelationship = null)
    {
        return new Faker<UserResult>()
            .RuleFor(x => x.Id, f => id.ToString() ?? Guid.NewGuid().ToString())
            .RuleFor(x => x.Email, f => email ?? f.Person.Email)
            .RuleFor(x => x.FirstName, f => firstName ?? f.Person.FirstName)
            .RuleFor(x => x.MiddleName, f => middleName)
            .RuleFor(x => x.LastName, f => lastName)
            .RuleFor(x => x.Role, f => role ?? RoleFactory.Member().Name)
            .RuleFor(x => x.AvatarName, f => avatar ?? f.Lorem.Sentence())
            .RuleFor(x => x.ViewerRelationship, f => viewerRelationship);
    }
}