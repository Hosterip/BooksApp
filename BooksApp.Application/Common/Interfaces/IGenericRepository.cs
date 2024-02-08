using System.Linq.Expressions;

namespace PostsApp.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetSingleWhereAsync(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
}