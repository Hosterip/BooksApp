using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostsApp.Models;

namespace PostsApp.Infrastructure.DB;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=HOSTERIP\\SQLEXPRESS;Database=postsapp;Trusted_Connection=True; TrustServerCertificate=True");
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}