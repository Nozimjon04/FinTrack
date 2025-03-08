using FinTrack.Api.Data.IRepositories;
using FinTrack.Api.Helpers;
using FinTrack.Api.Service.Mappers;
using FinTrack.Api.Data.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using FinTrack.Api.Service.Interfaces;
using FinTrack.Api.Service.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace FinTrack.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fintrack.Api", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
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
                    new string[] { }
                }
            });
        });
    }
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new ConfigureApiUrlName()));
        });

    }

    public static void InitAccessor(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        HttpContextHelper.Accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
    }

    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });
    }
}
