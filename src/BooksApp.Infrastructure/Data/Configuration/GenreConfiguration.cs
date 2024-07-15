using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Book;
using PostsApp.Domain.Genre;
using PostsApp.Domain.Genre.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => GenreId.CreateGenreId(value));

        builder
            .HasMany<Book>(g => g.Books)
            .WithMany(b => b.Genres)
            .UsingEntity<Dictionary<string, object>>(
                "BooksGenres",
                j => j.HasOne<Book>().WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Genre>().WithMany().OnDelete(DeleteBehavior.Cascade)
            );

    }
}