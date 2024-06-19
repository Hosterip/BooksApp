using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Review;
using PostsApp.Domain.Review.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ReviewId.CreateReviewId(value));
        
        builder.HasOne(r => r.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Book)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        // AutoIncludes
        builder.Property(r => r.Body)
            .HasMaxLength(1000)
            .IsRequired();
        builder.Navigation(r => r.User)
            .AutoInclude();
        builder.Navigation(r => r.Book)
            .AutoInclude();
    }
}