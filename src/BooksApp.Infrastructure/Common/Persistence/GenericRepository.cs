using System.Linq.Expressions;
using BooksApp.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Common.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Set<T>().ToListAsync(token);
    }

    public virtual async Task<IEnumerable<T>> GetAllWhereAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await _dbContext.Set<T>().Where(expression).ToListAsync(token);
    }

    public virtual async Task<T?> GetSingleWhereAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await _dbContext.Set<T>().SingleOrDefaultAsync(expression, cancellationToken: token);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await _dbContext.Set<T>().AnyAsync(expression, cancellationToken: token);
    }

    public async Task AddAsync(
        T entity,
        CancellationToken token = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken: token);
    }

    public void Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}