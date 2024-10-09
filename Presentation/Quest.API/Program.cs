using Serilog;
using Quest.Persistance;
using Quest.Persistance.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .WriteTo.Console() 
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .Enrich.FromLogContext() 
    .CreateLogger();

//DI service
builder.Services.AddPersistanceService();

builder.Services.AddControllers();

var app = builder.Build();
// Configure the HTTP request pipeline.

// Migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuestContext>(); 
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Произошла ошибка во время migrating the database.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
