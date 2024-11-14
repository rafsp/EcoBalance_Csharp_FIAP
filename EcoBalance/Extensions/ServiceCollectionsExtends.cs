using System.Reflection;
using EcoBalance.Configuration;
using EcoBalance.Database;
using EcoBalance.Models;
using EcoBalance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EcoBalance.Extensions
{
    public static class ServiceCollectionsExtends
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, APPConfiguration configuration)
        {
            services.AddDbContext<OracleDbContext>(options =>
            {
                options.UseOracle(configuration.OracleDatabase.Connection);
            });
            services.AddHttpClient<ThingerService>();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, APPConfiguration configuration)
        {

            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                    });

                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = configuration.Swagger.Title,
                    Description = configuration.Swagger.Description,
                    Contact = new OpenApiContact()
                    {
                        Email = configuration.Swagger.Email,
                        Name = configuration.Swagger.Name
                    }
                });

                var xmlFile = "Swagger.xml";  
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

            });

            return services;

        }
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, APPConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddOracle(configuration.OracleDatabase.Connection, name: configuration.OracleDatabase.Name);

            return services;
        }
    }
}
