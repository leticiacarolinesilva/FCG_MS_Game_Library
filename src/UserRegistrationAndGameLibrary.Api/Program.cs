using Microsoft.EntityFrameworkCore;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra;
using UserRegistrationAndGameLibrary.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserRegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameLibraryRepository, GameLibraryRepository>();

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

app.Run();
