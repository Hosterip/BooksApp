namespace BooksApp.Domain.Common.Interfaces;

public interface IPasswordHasher
{
    public bool IsPasswordValid(string userHash, string salt, string password);
    public (string Hash, string Salt) GenerateHashSalt(string password);
}