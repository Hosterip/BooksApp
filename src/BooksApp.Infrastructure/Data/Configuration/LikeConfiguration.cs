using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Models;

namespace PostsApp.Infrastructure.Data.Configuration;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasOne(l => l.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(l => l.Book)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}