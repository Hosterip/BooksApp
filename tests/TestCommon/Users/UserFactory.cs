using Bogus;
using BooksApp.Domain.Image;
using BooksApp.Domain.Role;
using BooksApp.Domain.User;
using TestCommon.Common.Factories;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        Role? role = null,
        string? email = null,
        string? password = null,
        string? firstName = null,
        string? middleName = null,
        string? lastName = null,
        Image? image = null)
    {
        return new Faker<User>()
            .CustomInstantiator(x =>
                User.Create(
                    PasswordHasherFactory.CreatePasswordHasher(),
                    email ?? x.Person.Email,
                    role ?? RoleFactory.Member(),
                    password ?? x.Lorem.Word(),
                    image,
                    firstName ?? x.Person.FirstName,
                    middleName,
                    lastName));
    }
}