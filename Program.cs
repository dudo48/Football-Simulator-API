using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FootballSimulatorAPI.Models;
using FootballSimulatorAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FootballSimulatorAPIContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("FootballSimulatorAPIContext") ?? throw new InvalidOperationException("Connection string 'FootballSimulatorAPIContext' not found.")).UseSnakeCaseNamingConvention());
//builder.Services.AddDbContext<FootballSimulatorAPIContext>(options => options.UseInMemoryDatabase("fsim"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
