using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BMedia.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BMedia.API"))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        // Create design-time services
        var currentUserService = new DesignTimeCurrentUserService();
        var dateTimeService = new DesignTimeDateTimeService();
        var auditInterceptor = new AuditableEntityInterceptor(currentUserService, dateTimeService);

        return new ApplicationDbContext(optionsBuilder.Options, auditInterceptor);
    }

    private class DesignTimeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId => null;
        public string? Email => null;
        public IEnumerable<string> Roles => Array.Empty<string>();
        public IEnumerable<string> Permissions => Array.Empty<string>();
        public bool HasPermission(string permission) => false;
        public bool IsAuthenticated => false;
    }

    private class DesignTimeDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
    }
}
