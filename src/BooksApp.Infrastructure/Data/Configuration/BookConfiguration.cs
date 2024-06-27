using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Book;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.User;

namespace PostsApp.Infrastructure.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BookId.CreateBookId(value));
        
        builder.Property(b => b.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(b => b.Description)
            .HasMaxLength(1000)
            .IsRequired();
        
        // AutoIncludes
        builder.Navigation(b => b.Author)
            .AutoInclude();
        builder.Navigation(b => b.Cover)
            .AutoInclude();
    }
}