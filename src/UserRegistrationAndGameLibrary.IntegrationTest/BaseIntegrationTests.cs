using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Testcontainers.PostgreSql;
using UserRegistrationAndGameLibrary.Api;
using UserRegistrationAndGameLibrary.Infra;
using Xunit;

namespace UserRegistrationAndGameLibrary.IntegrationTest;

public class BaseIntegrationTests : IAsyncLifetime
{
    protected HttpClient HttpClient { get; private set; }
    protected readonly PostgreSqlContainer DbContainer;
    protected UserRegistrationDbContext DbContext;
    private WebApplicationFactory<Program> _factory;

    public BaseIntegrationTests()
    {
        DbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot(
                    Path.Combine("src", "UserRegistrationAndGameLibrary.Api"));

                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<UserRegistrationDbContext>>();
                    services.AddDbContext<UserRegistrationDbContext>(options => 
                        options.UseNpgsql(DbContainer.GetConnectionString()));
                });
            });

        HttpClient = _factory.CreateClient();
    
        // Create a scope to resolve DbContext
        var scope = _factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<UserRegistrationDbContext>();
        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        if (DbContext != null)
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.DisposeAsync();
        }
        await DbContainer.DisposeAsync();
        HttpClient?.Dispose();
        _factory?.Dispose();
    }
}