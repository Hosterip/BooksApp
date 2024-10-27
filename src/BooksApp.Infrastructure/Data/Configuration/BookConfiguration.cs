using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Book;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Enums.MaxLengths;

namespace PostsApp.Infrastructure.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BookId.CreateBookId(value));

        builder.Property(b => b.Title)
            .HasMaxLength((int)BookMaxLengths.Title)
            .IsRequired();
        builder.Property(b => b.Description)
            .HasMaxLength((int)BookMaxLengths.Description)
            .IsRequired();

        // AutoIncludes
        builder.Navigation(b => b.Author)
            .AutoInclude();
        builder.Navigation(b => b.Genres)
            .AutoInclude();
        builder.Navigation(b => b.Cover)
            .AutoInclude();
    }
}