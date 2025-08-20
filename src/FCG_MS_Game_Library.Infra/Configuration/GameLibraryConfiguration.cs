using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Infra.Configuration;

public class GameLibraryConfiguration : IEntityTypeConfiguration<GameLibrary>
{
    public void Configure(EntityTypeBuilder<GameLibrary> builder)
    {
        // Table name and schema
        builder.ToTable("gamelibraries");

        // Primary Key
        builder.HasKey(gl => gl.Id);
        builder.Property(gl => gl.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        // Relationships
        builder.HasOne(gl => gl.Game)
            .WithMany()
            .HasForeignKey(gl => gl.GameId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Properties
        builder.Property(g => g.UserId)
            .HasColumnName("userId")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(gl => gl.PurchaseDate)
            .HasColumnName("purchase_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(gl => gl.PurchasePrice)
            .HasColumnName("purchase_price")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(gl => gl.IsInstalled)
            .HasColumnName("is_installed")
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(gl => gl.UserId)
            .HasDatabaseName("ix_game_libraries_user_id");

        builder.HasIndex(gl => gl.GameId)
            .HasDatabaseName("ix_game_libraries_game_id");

        builder.HasIndex(gl => new { gl.UserId, gl.GameId })
            .IsUnique()
            .HasDatabaseName("ux_game_libraries_user_game");

        // Complex constraints
        builder.HasCheckConstraint(
            "ck_game_libraries_positive_price",
            "purchase_price >= 0");
    }
}
