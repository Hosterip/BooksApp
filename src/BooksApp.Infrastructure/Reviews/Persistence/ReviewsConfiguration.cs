﻿using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Review;
using BooksApp.Domain.Review.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Reviews.Persistence;

public class ReviewsConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.User);
        builder.HasOne(r => r.Book);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ReviewId.Create(value));

        builder.HasOne(r => r.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Book)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        // AutoIncludes
        builder.Property(r => r.Body)
            .HasMaxLength(MaxPropertyLength.Review.Body)
            .IsRequired();
        builder.Navigation(r => r.User)
            .AutoInclude();
        builder.Navigation(r => r.Book)
            .AutoInclude();
    }
}