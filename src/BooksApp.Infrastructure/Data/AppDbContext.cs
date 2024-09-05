using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostsApp.Domain.Book;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.Bookshelf.Entities;
using PostsApp.Domain.Genre;
using PostsApp.Domain.Image;
using PostsApp.Domain.Review;
using PostsApp.Domain.Role;
using PostsApp.Domain.User;

namespace PostsApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=localhost;Database=booksapp;Trusted_Connection=True;TrustServerCertificate=True");
    } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasOne(b => b.Author);
        modelBuilder.Entity<Book>().HasOne(b => b.Cover);
        
        modelBuilder.Entity<Review>().HasOne(r => r.User);
        modelBuilder.Entity<Review>().HasOne(r => r.Book);

        modelBuilder.Entity<User>().HasOne(u => u.Role);
        
        modelBuilder.Entity<Role>().HasData(Role.Member());
        modelBuilder.Entity<Role>().HasData(Role.Author());
        modelBuilder.Entity<Role>().HasData(Role.Moderator());
        modelBuilder.Entity<Role>().HasData(Role.Admin());
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Review>  Reviews { get; set; }
    public DbSet<Image>  Images { get; set; }
    public DbSet<Genre>  Genres { get; set; }
    public DbSet<Bookshelf>  Bookshelves { get; set; }
    public DbSet<BookshelfBook>  BookshelfBooks { get; set; }
}