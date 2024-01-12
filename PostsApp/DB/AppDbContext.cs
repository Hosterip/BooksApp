using Microsoft.EntityFrameworkCore;
using PostsApp.Models;

namespace PostsApp.DB;

public class AppDbContext : DbContext
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