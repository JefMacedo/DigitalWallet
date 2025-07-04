using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Application.Validators;
using DigitalWallet.Infrastructure;
using DigitalWallet.Infrastructure.Security;
using DigitalWallet.Persistence.Context;
using DigitalWallet.Persistence.Repositories;
using DigitalWallet.API.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DigitalWalletDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

// Swagger com suporte a JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Digital Wallet API",
        Version = "v1",
        Description = "API da Carteira Digital",
        Contact = new OpenApiContact
        {
            Name = "Jeferson Macedo",
            Email = "jhef.salles@gmail.com"
        }
    });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    opt.AddSecurityDefinition("Bearer", jwtScheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// App Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// CORS liberando para desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

// Global Exception Middleware
app.UseGlobalExceptionHandling();

app.UseCors("DevPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Wallet API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
