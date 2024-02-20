using PostsApp.Domain.Constants;

namespace PostsApp.Domain.Common;

public static class RolePermissions
{
    public static bool UpdateRole(string changerUserRoleName, string targetUserRoleName, string roleNameToChange)
    {
        // a Member or an Author can't promote someone 
        if (changerUserRoleName == RoleConstants.Member ||
            changerUserRoleName == RoleConstants.Author)
            return false;
        // Moderator can't promote someone to an Admin
        if (changerUserRoleName == RoleConstants.Moderator &&
            roleNameToChange == RoleConstants.Admin)
            return false;
        // Moderator can't change role of an Admin
        if (changerUserRoleName == RoleConstants.Moderator &&
            targetUserRoleName == RoleConstants.Admin)
            return false;
        // Admin can promote or demote every one
        return changerUserRoleName == RoleConstants.Admin;
    }
    
    public static bool UpdateOrDeleteBook(string changerUserRoleName)
    {
        // Only Admin, Moderator or Author of that book can change it. 
        return changerUserRoleName == RoleConstants.Admin
               || changerUserRoleName == RoleConstants.Moderator
               || changerUserRoleName == RoleConstants.Author;
    }
    public static bool CreateBookBook(string changerUserRoleName)
    {
        // Only Authors can create a book. 
        return changerUserRoleName == RoleConstants.Author;
    }
}