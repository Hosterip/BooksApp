using System.Linq.Expressions;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetSingleWhereAsync(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    void Remove(T entity);
}