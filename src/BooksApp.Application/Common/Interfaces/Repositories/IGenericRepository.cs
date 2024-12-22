using System.Linq.Expressions;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default);
    Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<T?> GetSingleWhereAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);
    Task AddAsync(T entity, CancellationToken token = default);
    Task Remove(T entity);
    Task Update(T entity);
}