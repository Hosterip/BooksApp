namespace BooksApp.Application.Common.Interfaces;

public interface IUserService
{
    public string? GetEmail();
    public string? GetRole();
    public Guid? GetId();
    public string? GetSecurityStamp();
    public void ChangeEmail(string valueOfClaim);
    public void ChangeRole(string valueOfClaim);
    public void ChangeSecurityStamp(string valueOfClaim);
    public Task Login(string id, string email, string role, string securityStamp);
    public Task Logout();
}