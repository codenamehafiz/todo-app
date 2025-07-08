using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Application.Validators;
using TodoApp.Infrastructure;
using TodoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo API",
        Version = "v1",
        Description = "A simple Todo API built with Clean Architecture principles",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add FluentValidation
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateTodoItemValidator>();
});

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();