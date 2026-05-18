using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.BackgroundJobs;
using BMedia.Infrastructure.Persistence;
using BMedia.Infrastructure.Persistence.Interceptors;
using BMedia.Infrastructure.Services.Auth;
using BMedia.Infrastructure.Services.Cache;
using BMedia.Infrastructure.Services.Media;
using BMedia.Infrastructure.Services.Notification;
using BMedia.Infrastructure.Services.Search;
using BMedia.Infrastructure.Services.Storage;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMedia.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                });
            options.UseSnakeCaseNamingConvention();
        });

        // Redis
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Redis");
            opt.InstanceName = "BMedia:";
        });

        // Hangfire
        services.AddHangfire(cfg => cfg
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))));
        services.AddHangfireServer();

        // Auth services
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

        // Storage (swap provider via config)
        services.Configure<LocalStorageOptions>(configuration.GetSection("Storage:Local"));
        services.AddScoped<IStorageService, LocalStorageService>();

        // Domain services
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISearchService, StubSearchService>();
        services.AddScoped<IMediaProcessingService, MediaProcessingService>();

        // Background jobs
        services.AddScoped<MediaProcessingJobs>();
        services.AddScoped<ScheduledPublicationJob>();

        return services;
    }
}
