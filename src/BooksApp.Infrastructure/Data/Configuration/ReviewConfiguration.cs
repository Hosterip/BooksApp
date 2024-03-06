using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Models;

namespace PostsApp.Infrastructure.Data.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Body)
            .HasMaxLength(1000)
            .IsRequired();
        builder.HasOne(r => r.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Book)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}