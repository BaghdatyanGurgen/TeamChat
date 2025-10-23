using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TeamChat.Application;
using TeamChat.Infrastructure;
using TeamChat.Infrastructure.Email;
using TeamChat.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// =========================
// CONFIGURATION
// =========================
var configuration = builder.Configuration;

// =========================
// DEPENDENCY INJECTION
// =========================
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();    // Application layer
builder.Services.AddInfrastructure(configuration); // Infrastructure + Persistence
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

// =========================
// CONTROLLERS
// =========================
builder.Services.AddControllers();

// =========================
// SWAGGER
// =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TeamChat API",
        Version = "v1"
    });
});

// =========================
// APP PIPELINE
// =========================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
