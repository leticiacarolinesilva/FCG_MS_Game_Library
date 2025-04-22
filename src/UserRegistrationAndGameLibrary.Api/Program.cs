
using Microsoft.EntityFrameworkCore;

using UserRegistrationAndGameLibrary.Api.Extensions;

using UserRegistrationAndGameLibrary.Api.Services;
using UserRegistrationAndGameLibrary.Api.Services.Interfaces;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra;
using UserRegistrationAndGameLibrary.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserRegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameLibraryRepository, GameLibraryRepository>();
builder.Services.AddScoped<ICorrelationIdGeneratorService, CorrelationIdGeneratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Middlewares
app.UseMiddlewareExtensions();
#endregion

app.Run();
