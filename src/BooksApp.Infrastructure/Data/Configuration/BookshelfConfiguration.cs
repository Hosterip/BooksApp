using BooksApp.Domain.Book;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Enums.MaxLengths;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Data.Configuration;

public class BookshelfConfiguration : IEntityTypeConfiguration<Bookshelf>
{
    public void Configure(EntityTypeBuilder<Bookshelf> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BookshelfId.CreateBookshelfId(value));

        builder.Property(g => g.Name)
            .HasMaxLength((int)BookshelfMaxLengths.Name)
            .IsRequired();

        builder.HasOne(b => b.User)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.OwnsMany(b => b.BookshelfBooks, bb =>
        {
            bb.HasKey(b => b.Id);

            bb.HasOne<Book>(b => b.Book)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            bb.Property(o => o.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => BookshelfBookId.CreateBookshelfBookId(value));

            bb.Navigation(b => b.Book)
                .AutoInclude();
        });

        builder.Navigation(b => b.User)
            .AutoInclude();
    }
}