﻿using BooksApp.Domain.Book;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Constants.MaxLengths;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Bookshelves.Persistence;

public class BookshelvesConfiguration : IEntityTypeConfiguration<Bookshelf>
{
    public void Configure(EntityTypeBuilder<Bookshelf> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BookshelfId.Create(value));

        builder.Property(g => g.Name)
            .HasMaxLength(MaxPropertyLength.Bookshelf.Name)
            .IsRequired();

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
                    value => BookshelfBookId.Create(value));

            bb.Navigation(b => b.Book)
                .AutoInclude();
        });
    }
}