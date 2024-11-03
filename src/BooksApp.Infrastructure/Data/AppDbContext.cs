using System.Reflection;
using BooksApp.Domain.Book;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Image;
using BooksApp.Domain.Review;
using BooksApp.Domain.Role;
using BooksApp.Domain.User;
using BooksApp.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BooksApp.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
        var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        if (databaseCreator == null) return;
        if (!databaseCreator.CanConnect()) databaseCreator.Create();
        if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        Database.GetAppliedMigrations();
    }    
    
    public DbSet<User> Users { get; init; }
    public DbSet<Relationship> Relationships { get; init; }
    public DbSet<Book> Books { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<Review> Reviews { get; init; }
    public DbSet<Image> Images { get; init; }
    public DbSet<Genre> Genres { get; init; }
    public DbSet<Bookshelf> Bookshelves { get; init; }
    public DbSet<BookshelfBook> BookshelfBooks { get; init; }

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
}