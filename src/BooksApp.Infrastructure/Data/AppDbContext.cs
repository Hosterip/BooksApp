using System.Reflection;
using BooksApp.Domain.Book;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Image;
using BooksApp.Domain.Review;
using BooksApp.Domain.Role;
using BooksApp.Domain.User;
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

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Bookshelf> Bookshelves { get; set; }
    public DbSet<BookshelfBook> BookshelfBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
        optionsBuilder
            .UseSqlServer(
                $"Server={dbHost};Database={dbName};User Id=sa;Password={dbPassword};Trusted_Connection=False;TrustServerCertificate=True");
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
}