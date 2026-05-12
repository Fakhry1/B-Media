using BMedia.Domain.Enums;

namespace BMedia.Domain.Interfaces;

public interface INotificationService
{
    Task SendAsync(Guid userId, NotificationType type, string title, string message,
        Guid? referenceId = null, string? referenceType = null, string? actionUrl = null,
        CancellationToken cancellationToken = default);

    Task SendBulkAsync(IEnumerable<Guid> userIds, NotificationType type, string title, string message,
        CancellationToken cancellationToken = default);
}
