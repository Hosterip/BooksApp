using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.Role;
using PostsApp.Domain.Role.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.Property(r => r.Name)
            .HasMaxLength((int)RoleMaxLengths.Name)
            .IsRequired();

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => RoleId.CreateRoleId(value));
    }
}