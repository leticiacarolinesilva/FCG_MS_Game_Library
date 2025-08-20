using Microsoft.EntityFrameworkCore;

using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Infra;

public class UserRegistrationDbContext : DbContext
{
    public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// Used for EF Core
    /// </summary>
    protected UserRegistrationDbContext()
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameLibrary> GameLibraries => Set<GameLibrary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default schema for all entities
        modelBuilder.HasDefaultSchema("game_platform");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserRegistrationDbContext).Assembly);

        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.Entity<GameLibrary>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        });

        base.OnModelCreating(modelBuilder);
    }
}
