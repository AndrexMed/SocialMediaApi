using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System.Reflection;

namespace SocialMedia.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            //Conexion bd
            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(configuration
                        .GetConnectionString("stringCnx"))
                        .EnableSensitiveDataLogging());

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //Para acceder a las variables del settings.
            services.Configure<PaginationOptions>(configuration.GetSection("Pagination"));
            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));

            return services;
        }

        public static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            //Injection dependencies
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IPostService, PostService>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            services.AddSingleton<IPasswordService, PasswordService>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, string xmlFileName)
        {
            //Swagger configuration
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                doc.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
                doc.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}
