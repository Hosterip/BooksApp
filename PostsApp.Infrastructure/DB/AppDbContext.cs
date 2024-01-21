using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;
using PostsApp.Models;

namespace PostsApp.Infrastructure.DB;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}