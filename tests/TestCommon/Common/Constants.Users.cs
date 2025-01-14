using BooksApp.Domain.Role;

namespace TestCommon.Common;

public static partial class Constants
{
    public static class Users
    {
        public const string Email = "foo@foo.com";
        
        public const string FirstName = "foo";
        public const string MiddleName = "foo";
        public const string LastName = "foo";
        
        public const string Password = "foo";
        
        public static Role Role => RoleFactory.Member();
    }
}