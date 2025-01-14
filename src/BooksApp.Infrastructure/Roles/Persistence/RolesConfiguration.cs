using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Role;
using BooksApp.Domain.Role.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksApp.Infrastructure.Roles.Persistence;

public class RolesConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.Property(r => r.Name)
            .HasMaxLength(RoleMaxLengths.Name)
            .IsRequired();

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => RoleId.CreateRoleId(value));
        
        builder.HasData(RoleFactory.Member());
        builder.HasData(RoleFactory.Author());
        builder.HasData(RoleFactory.Moderator());
        builder.HasData(RoleFactory.Admin());
    }
}