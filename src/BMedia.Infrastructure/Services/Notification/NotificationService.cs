using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace BMedia.Infrastructure.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ApplicationDbContext db, ILogger<NotificationService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SendAsync(Guid userId, NotificationType type, string title, string message,
        Guid? referenceId = null, string? referenceType = null, string? actionUrl = null,
        CancellationToken cancellationToken = default)
    {
        var notification = new Domain.Entities.Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            ActionUrl = actionUrl
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, title);
    }

    public async Task SendBulkAsync(IEnumerable<Guid> userIds, NotificationType type, string title, string message,
        CancellationToken cancellationToken = default)
    {
        var notifications = userIds.Select(uid => new Domain.Entities.Notification
        {
            UserId = uid,
            Type = type,
            Title = title,
            Message = message
        }).ToList();

        _db.Notifications.AddRange(notifications);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
