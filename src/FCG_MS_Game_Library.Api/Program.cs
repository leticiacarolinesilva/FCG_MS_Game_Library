using FCG_MS_Game_Library.Infra;

using FCG_MS_Users.Api.Extensions;

using Microsoft.EntityFrameworkCore;

using Nest;

using UserRegistrationAndGameLibrary.Api.Extensions;
using UserRegistrationAndGameLibrary.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserRegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");

var userUri = builder.Configuration["UserClient:Uri"];

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

if (string.IsNullOrEmpty(userUri))
{
    throw new ArgumentException("User Client URI is not configured.", nameof(userUri));
}

builder.Services.UseCollectionExtensions(userUri);

builder.Services.UseAuthenticationExtensions(jwtKey);

builder.Services.UseSwaggerExtensions();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "PostgreSQL");

var elasticUri = builder.Configuration["Elastic:Uri"];
var elasticUser = builder.Configuration["Elastic:Username"];
var elasticPass = builder.Configuration["Elastic:Password"];

if (string.IsNullOrEmpty(elasticUri))
{
    throw new ArgumentException("Elasticsearch URI is not configured.", nameof(elasticUri));
}

builder.Services.AddSingleton<IElasticClient>(sp =>
    ElasticSearchClientFactory.CreateClient(elasticUri, elasticUser, elasticPass));

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserRegistrationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#region Middlewares
app.UseMiddlewareExtensions();
#endregion

app.Run();


public partial class Program { }
