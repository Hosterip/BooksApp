using BooksApp.Domain.Common.Enums.MaxLengths;
using BooksApp.Domain.User;
using BooksApp.Domain.User.Entities;
using BooksApp.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Users.Persistence;

public class UsersConfiguration : IEntityTypeConfiguration<User>
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

        builder.HasOne(u => u.Role);

        builder.HasMany<Relationship>(u => u.Followers)
            .WithOne()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany<Relationship>(u => u.Following)
            .WithOne()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property("Hash")
            .IsRequired();
        builder.Property("Salt")
            .IsRequired();
        // AutoIncludes
        builder
            .Navigation(u => u.Avatar)
            .AutoInclude();
        builder
            .Navigation(u => u.Followers)
            .AutoInclude();
        builder
            .Navigation(u => u.Role)
            .AutoInclude();
    }
}