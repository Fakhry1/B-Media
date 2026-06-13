using BMedia.API.Extensions;
using BMedia.API.Middleware;
using BMedia.Application;
using BMedia.Infrastructure.Extensions;
using BMedia.Infrastructure.Persistence;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File("logs/bmedia-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30));

// Application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Log JWT configuration at startup to verify settings are loaded correctly
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
var jwtSection = builder.Configuration.GetSection("Jwt");
startupLogger.LogInformation(
    "JWT config — Issuer={Issuer} Audience={Audience} KeyConfigured={HasKey} KeyLength={KeyLength}",
    jwtSection["Issuer"],
    jwtSection["Audience"],
    !string.IsNullOrEmpty(jwtSection["SecretKey"]),
    jwtSection["SecretKey"]?.Length ?? 0);

// Apply pending EF migrations on startup (non-fatal — DB may be pre-seeded via SQL script)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");

        // Seed default admin user if no users exist
        if (!db.Users.Any())
        {
            var hasher = scope.ServiceProvider.GetRequiredService<BMedia.Domain.Interfaces.IPasswordHasher>();
            var adminRoleId = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var adminUserId = Guid.Parse("A0000000-0000-0000-0000-000000000001");

            var admin = new BMedia.Domain.Entities.User
            {
                Id = adminUserId,
                Username = "admin",
                Email = "admin@bmedia.io",
                PasswordHash = hasher.Hash("Admin@1234"),
                FirstName = "Admin",
                LastName = "User",
                IsActive = true,
                IsEmailVerified = true,
                PreferredLanguage = "ar",
            };

            var adminRole = await db.Roles.FindAsync(adminRoleId);
            if (adminRole is not null)
            {
                admin.UserRoles = [new BMedia.Domain.Entities.UserRole { UserId = adminUserId, RoleId = adminRoleId }];
            }

            db.Users.Add(admin);
            await db.SaveChangesAsync();
            logger.LogInformation("Default admin user seeded — Email: admin@bmedia.io / Password: Admin@1234");
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Migration/seed step skipped — database may already be at the latest schema version");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger — always enabled so it is reachable during development regardless of HTTPS cert trust
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "BMedia API v1");
    opt.RoutePrefix = "swagger";          // accessible at /swagger
    opt.DocumentTitle = "BMedia API";
    opt.DisplayRequestDuration();
    opt.EnableDeepLinking();
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    await next();
});

app.UseSerilogRequestLogging(opt =>
{
    opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId", httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "anonymous");
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

app.UseCors("DefaultCors");
app.UseRateLimiter();
app.UseAuthentication();

// Diagnostic: log which endpoint is matched and its auth metadata
app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint is not null)
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("EndpointDiag");
        var hasAllowAnon = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() is not null;
        var hasAuthorize = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAuthorizeData>() is not null;
        logger.LogInformation("DIAG Endpoint={Name} AllowAnonymous={Anon} HasAuthorize={Auth}",
            endpoint.DisplayName, hasAllowAnon, hasAuthorize);
    }
    await next();
});

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthorizationFilter()]
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.Run();

public partial class Program { }
