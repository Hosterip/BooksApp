using Microsoft.EntityFrameworkCore;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}