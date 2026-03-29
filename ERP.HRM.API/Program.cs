using ERP.HRM.API;
using ERP.HRM.API.Configuration;
using ERP.HRM.API.HealthChecks;
using ERP.HRM.API.Middlewares;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Application.Mappings;
using ERP.HRM.Application.Services;
using ERP.HRM.Application.Validators;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using ERP.HRM.Infrastructure;
using ERP.HRM.Infrastructure.Repositories;
using ERP.HRM.Infrastructure.Seed;
using ERP.HRM.Infrastructure.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

try
{
    Log.Information("Starting application...");

    // Serilog
    builder.Host.UseSerilog();

    // DbContext
    builder.Services.AddDbContext<ERPDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.CommandTimeout(30)));

    // Identity
    builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<ERPDbContext>()
    .AddDefaultTokenProviders();

    // Validation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();

    // MediatR - CQRS
    builder.Services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssembly(typeof(ERP.HRM.Application.Features.Departments.Commands.CreateDepartmentCommand).Assembly));

    // Unit of Work
    builder.Services.AddScoped<IUnitOfWork>(provider => 
        new UnitOfWork(provider.GetRequiredService<ERPDbContext>()));

    // Repositories
    builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IPositionRepository, PositionRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

    // Services
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IPositionService, PositionService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // AutoMapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // Caching
    builder.Services.AddMemoryCache();

    // JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")))
        };
    });

    // Authorization
    builder.Services.AddAuthorization();

    // CORS
    builder.Services.AddCustomCors(builder.Configuration);

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("Database");

    // Controllers
    builder.Services.AddControllers();

    // Swagger + JWT support
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ERP HRM API",
            Version = "v1",
            Description = "Enterprise Resource Planning - Human Resource Management API",
            Contact = new OpenApiContact
            {
                Name = "Development Team",
                Url = new Uri("https://github.com/nghixuanpham98/ERP.HRM.API")
            }
        });

        // JWT Security Definition
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT token: Bearer {token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
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

        // XML comments for documentation
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    });

    var app = builder.Build();

    // Pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP HRM API v1");
        });
    }

    // Apply CORS
    app.UseCustomCors(app.Environment);

    // Health checks endpoint
    app.MapHealthChecks("/health");

    // Database seeding
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            await DatabaseSeeder.SeedRolesAndAdminAsync(roleManager, userManager, logger);
            Log.Information("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during database seeding");
            throw;
        }
    }

    // Middleware Pipeline
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseMiddleware<AuditLoggingMiddleware>();
    app.UseMiddleware<RateLimitingMiddleware>();
    app.UseMiddleware<GlobalException>();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Application started successfully");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
