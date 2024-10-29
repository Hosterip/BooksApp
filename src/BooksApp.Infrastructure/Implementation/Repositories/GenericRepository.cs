using System.Linq.Expressions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbContext.Set<T>().Where(expression).ToListAsync();
    }

    public virtual async Task<T?> GetSingleWhereAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbContext.Set<T>().SingleOrDefaultAsync(expression);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbContext.Set<T>().AnyAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public void Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}