using BooksApp.Domain.Image;
using BooksApp.Domain.Role;
using BooksApp.Domain.User;
using TestCommon.Common;
using TestCommon.Common.Factories;

namespace TestCommon.Users;

public static class UserFactory
{
    public static User CreateUser(
        Role? role = null, 
        string email = Constants.Users.Email,
        string password = Constants.Users.Password,
        string firstName = Constants.Users.FirstName,
        string? middleName = null,
        string? lastName = null,
        Image? image = null)
    {
        return User.Create(
            PasswordHasherFactory.CreatePasswordHasher(),
            email,
            role ?? Constants.Users.Role,
            password,
            image,
            firstName,
            middleName,
            lastName);
    }
}