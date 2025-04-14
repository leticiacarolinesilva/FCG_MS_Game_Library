using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Infra.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("games");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(g => g.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasColumnType("text");

        builder.Property(g => g.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(g => g.ReleasedDate)
            .HasColumnName("release_date")
            .IsRequired();

        // Configure enum conversion
        builder.Property(g => g.Genre)
            .HasColumnName("genre")
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(g => g.CoverImageUrl)
            .HasColumnName("cover_image_url")
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(g => g.Title).IsUnique();
        builder.HasIndex(g => g.Genre);
        builder.HasIndex(g => g.ReleasedDate);
    }
}