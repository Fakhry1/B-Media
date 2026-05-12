using BMedia.Domain.Enums;
using BMedia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BMedia.Infrastructure.BackgroundJobs;

public class ScheduledPublicationJob
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ScheduledPublicationJob> _logger;

    public ScheduledPublicationJob(ApplicationDbContext db, ILogger<ScheduledPublicationJob> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task PublishContentAsync(Guid scheduledPublicationId, CancellationToken cancellationToken)
    {
        var scheduled = await _db.ScheduledPublications
            .Include(s => s.Content)
            .FirstOrDefaultAsync(s => s.Id == scheduledPublicationId, cancellationToken);

        if (scheduled is null || scheduled.IsExecuted) return;

        try
        {
            scheduled.Content.Status = ContentStatus.Published;
            scheduled.Content.PublishedAt = DateTime.UtcNow;
            scheduled.IsExecuted = true;
            scheduled.ExecutedAt = DateTime.UtcNow;
            scheduled.IsSuccessful = true;

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Content {ContentId} published via scheduled job", scheduled.ContentId);
        }
        catch (Exception ex)
        {
            scheduled.IsExecuted = true;
            scheduled.ExecutedAt = DateTime.UtcNow;
            scheduled.IsSuccessful = false;
            scheduled.ErrorMessage = ex.Message;
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogError(ex, "Failed to publish content {ContentId}", scheduled.ContentId);
            throw;
        }
    }
}
