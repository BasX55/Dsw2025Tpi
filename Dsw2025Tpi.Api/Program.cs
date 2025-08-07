using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace Dsw2025Tpi.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var path = "C:\\Log\\Tpi.txt";
        builder.Services.AddControllers();
        builder.Services.AddLogging(config =>
        {
            config.AddConsole()
            .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error)
            .AddFilter("Microsoft.AspNetCore", LogLevel.Information);

            config.AddFile(path);

        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Dsw2025Tpi"));
        }
        );
        builder.Services.AddSwaggerGen( o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dsw2025Tpi API",
                Version = "v1",
            });
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Please enter a valid token",
                Type = SecuritySchemeType.ApiKey


            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            o.CustomSchemaIds(type => type.FullName.Replace("+", "."));
        } );
        builder.Services.AddHealthChecks();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
        {
            option.Password = new PasswordOptions
            {
                RequiredLength = 8,

            };
        })
            .AddEntityFrameworkStores<AuthenticateContext>()
            .AddDefaultTokenProviders();

        var jwtConfig = builder.Configuration.GetSection("Jwt");
        var keyText = jwtConfig["Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured");
        var key = Encoding.UTF8.GetBytes(keyText);
        builder.Services.AddAuthentication(option =>
        {

            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = jwtConfig["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        

        builder.Services.AddScoped<Dsw2025Tpi.Domain.Interfaces.IRepository, Dsw2025Tpi.Data.Repositories.EfRepository>(); 
        
        builder.Services.AddScoped<ProductsManagementService>();
        builder.Services.AddScoped<OrderManagementService>();
        builder.Services.AddSingleton<JwtTokenService>();
        builder.Services.AddScoped<CustomerManagementService>();




        builder.Services.AddDbContext<AuthenticateContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Dsw2025Tpi"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.MapHealthChecks("/healthcheck");

        app.Run();
    }
}
