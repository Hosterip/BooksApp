using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.User;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.CreateUserId());

        builder.Property(u => u.Username)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasIndex(u => u.Username)
            .IsUnique();
        builder.Property(u => u.Hash)
            .IsRequired();
        builder.Property(u => u.Salt)
            .IsRequired();
        // AutoIncludes
        builder
            .Navigation(u => u.Avatar)
            .AutoInclude();
        builder
            .Navigation(u => u.Role)
            .AutoInclude();
    }
}