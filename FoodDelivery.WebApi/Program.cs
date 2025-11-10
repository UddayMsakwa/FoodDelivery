using FoodDelivery.BusinessLogic.Interfaces;
using FoodDelivery.BusinessLogic.Services;
using FoodDelivery.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// DATABASE CONFIGURATION
// =======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// =======================================================
// JWT AUTHENTICATION CONFIGURATION
// =======================================================
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        // Diagnostic logging during development
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[JWT ERROR] {ctx.Exception.Message}");
                Console.ResetColor();
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[JWT VALIDATED]");
                Console.ResetColor();
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// =======================================================
// DEPENDENCY INJECTION
// =======================================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IDishCategoryService, DishCategoryService>();

builder.Services.AddControllers();

// =======================================================
// SWAGGER CONFIGURATION (WITH JWT SUPPORT)
// =======================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Food Delivery API",
        Version = "v1",
        Description = "Backend API for the Food Delivery project"
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Enter your JWT token here (example: Bearer {token})",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// =======================================================
// BUILD & RUN
// =======================================================
var app = builder.Build();

// =======================================================
// GLOBAL EXCEPTION LOGGER (for debugging)
// =======================================================
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[UNHANDLED ERROR]");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        Console.ResetColor();
        throw;
    }
});

// =======================================================
// PIPELINE
// =======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
