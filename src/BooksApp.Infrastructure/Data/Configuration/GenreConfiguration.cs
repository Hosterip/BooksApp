using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Book;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.Genre;
using PostsApp.Domain.Genre.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .HasMaxLength((int)GenreMaxLengths.Name)
            .IsRequired();

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => GenreId.CreateGenreId(value));

        builder
            .HasMany(genre => genre.Books)
            .WithMany(book => book.Genres)
            .UsingEntity<Dictionary<string, object>>(
                "BooksGenres",
                j => j.HasOne<Book>().WithMany().OnDelete(DeleteBehavior.NoAction));
    }
}