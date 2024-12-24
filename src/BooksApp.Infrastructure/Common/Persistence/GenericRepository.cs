using System.Linq.Expressions;
using BooksApp.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Common.Persistence;

public abstract class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T>
    where T : class
{
    protected readonly AppDbContext DbContext = dbContext;

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default)
    {
        return await DbContext.Set<T>()
            .ToListAsync(token);
    }

    public virtual async Task<IEnumerable<T>> GetAllWhereAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Set<T>()
            .Where(expression)
            .ToListAsync(token);
    }

    public virtual async Task<T?> GetSingleWhereAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Set<T>()
            .SingleOrDefaultAsync(expression, cancellationToken: token);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken token = default)
    {
        return await DbContext.Set<T>()
            .AnyAsync(expression, cancellationToken: token);
    }

    public async Task AddAsync(
        T entity,
        CancellationToken token = default)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken: token);
    }

    public Task Remove(T entity)
    {
        DbContext.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public virtual Task Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
        
        return Task.CompletedTask;
    }
}