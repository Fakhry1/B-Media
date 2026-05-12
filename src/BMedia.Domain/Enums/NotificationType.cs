namespace BMedia.Domain.Enums;

public enum NotificationType
{
    WorkflowTransition = 1,
    ReviewAssigned = 2,
    ReviewCompleted = 3,
    ContentPublished = 4,
    ContentRejected = 5,
    MediaProcessingComplete = 6,
    MediaProcessingFailed = 7,
    SystemAlert = 8,
    ScheduledPublicationDue = 9
}
