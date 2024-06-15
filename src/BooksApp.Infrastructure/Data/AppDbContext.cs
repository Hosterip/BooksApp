using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostsApp.Domain.Book;
using PostsApp.Domain.Image;
using PostsApp.Domain.Review;
using PostsApp.Domain.Role;
using PostsApp.Domain.User;

namespace PostsApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=HOSTERIP\\SQLEXPRESS;Database=postsapp;Trusted_Connection=True;TrustServerCertificate=True");
        
    } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Review>  Reviews { get; set; }
    public DbSet<Image>  Images { get; set; }
}