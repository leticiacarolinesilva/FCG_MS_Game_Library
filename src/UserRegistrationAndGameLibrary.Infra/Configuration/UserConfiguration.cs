using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Infra.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(u => u.Email, e =>
        {
            e.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Password, p =>
        {
            p.Property(pass => pass.HasedValue)
                .HasColumnName("password_hash")
                .IsRequired();
        });

        builder.Property(u => u.CreateAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()") 
            .IsRequired();

        builder.HasMany(u => u.GameLibrary)
            .WithOne(gl => gl.User)
            .HasForeignKey(gl => gl.UserId);

        builder.Property(u => u.Permission)
            .HasColumnName("permission")
            .HasConversion<string>() 
            .IsRequired();
    }
}
