using BooksApp.Domain.Common.Constants;

namespace BooksApp.Domain.Common.Security;

public static class RolePermissions
{
    public static bool UpdateRole(string changerUserRoleName, string targetUserRoleName, string roleNameToChange)
    {
        // a Member or an Author can't change someone's role 
        if (changerUserRoleName == RoleNames.Member ||
            changerUserRoleName == RoleNames.Author)
            return false;
        // Moderator can't promote someone to an Admin
        if (changerUserRoleName == RoleNames.Moderator &&
            roleNameToChange == RoleNames.Admin)
            return false;
        // Moderator can't change role of an Admin
        if (changerUserRoleName == RoleNames.Moderator &&
            targetUserRoleName == RoleNames.Admin)
            return false;
        // Admin can change someone's role
        return changerUserRoleName == RoleNames.Admin;
    }

    public static bool UpdateOrDeleteBook(string changerUserRoleName)
    {
        // Only Admin, Moderator or Author of that book can change it. 
        return changerUserRoleName == RoleNames.Admin
               || changerUserRoleName == RoleNames.Moderator
               || changerUserRoleName == RoleNames.Author;
    }

    public static bool CreateBook(string userRoleName)
    {
        // Only Authors can create a book. 
        return userRoleName == RoleNames.Author;
    }

    public static bool DeleteReview(string changerUserRoleName)
    {
        // Only Admin, Moderator or Author of that book can change it. 
        return changerUserRoleName == RoleNames.Admin
               || changerUserRoleName == RoleNames.Moderator;
    }
}