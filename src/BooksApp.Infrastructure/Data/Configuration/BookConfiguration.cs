using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Models;

namespace PostsApp.Infrastructure.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(b => b.Description)
            .HasMaxLength(1000)
            .IsRequired();
        // AutoIncludes
        builder.Navigation(b => b.Author)
            .AutoInclude();
        builder.Navigation(b => b.Cover)
            .AutoInclude();
    }
}