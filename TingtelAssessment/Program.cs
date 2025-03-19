using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.File;
using Serilog.Extensions.Hosting; // Add this using directive
using System.Reflection;
using TingtelAssessment.Application.Commands.CreateCommand;
using TingtelAssessment.Application.Interfaces;
using TingtelAssessment.Domain.StateMachine;
using TingtelAssessment.Infrastructure.Data;
using TingtelAssessment.Infrastructure.Repositories;
using TingtelAssessment.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/taskmanagementapi-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog(); // This should work now with the correct using directive
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "A microservices-based API for managing tasks",
        Contact = new OpenApiContact
        {
            Name = "API Developer",
            Email = "developer@example.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "TaskManagementApi:";
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<TaskStateMachine>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IMessageQueueService, RabbitMQService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        Log.Information("Database migrated successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating the database");
    }
}

app.Run();
