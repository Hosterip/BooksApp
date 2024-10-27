using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Common.Enums.MaxLengths;
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
                value => UserId.CreateUserId(value));

        builder.HasIndex(u => u.MiddleName);
        builder.HasIndex(u => u.LastName);
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasMaxLength((int)UserMaxLengths.Email);
        builder.Property(u => u.FirstName)
            .HasMaxLength((int)UserMaxLengths.FirstName)
            .IsRequired();
        builder.Property(u => u.MiddleName)
            .HasMaxLength((int)UserMaxLengths.MiddleName);
        builder.Property(u => u.LastName)
            .HasMaxLength((int)UserMaxLengths.LastName);

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