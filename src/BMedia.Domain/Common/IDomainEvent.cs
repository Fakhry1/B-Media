using MediatR;

namespace BMedia.Domain.Common;

/// <summary>Marker interface for domain events.</summary>
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
