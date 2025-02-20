using BooksApp.Domain.Common.Constants.MaxLengths;
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
                value => UserId.Create(value));

        builder.HasIndex(u => u.MiddleName);
        builder.HasIndex(u => u.LastName);
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Email)
            .HasMaxLength(MaxPropertyLength.User.Email);
        builder.Property(u => u.FirstName)
            .HasMaxLength(MaxPropertyLength.User.FirstName)
            .IsRequired();
        builder.Property(u => u.MiddleName)
            .HasMaxLength(MaxPropertyLength.User.MiddleName);
        builder.Property(u => u.LastName)
            .HasMaxLength(MaxPropertyLength.User.LastName);

        builder.HasOne(u => u.Role)
            .WithMany();

        builder.HasMany<Relationship>(u => u.Followers)
            .WithOne()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        
        builder.HasMany<Relationship>(u => u.Following)
            .WithOne()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        
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