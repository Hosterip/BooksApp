using BooksApp.Domain.Book;
using BooksApp.Domain.Common.Enums.MaxLengths;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Genre.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Data.Configuration;

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