﻿namespace PostsApp.Common.Extensions;

public static class SessionUserExtension
{
    public static void SetUserInSession(this ISession session, string username)
    {
        session.SetString("Username", username);
    } 
    
    public static string? GetUserInSession(this ISession session)
    {
        return session.GetString("Username");
    } 
    
    public static void RemoveUserInSession(this ISession session)
    {
        session.Remove("Username");
    } 
}