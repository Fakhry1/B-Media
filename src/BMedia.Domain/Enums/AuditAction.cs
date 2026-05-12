namespace BMedia.Domain.Enums;

public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    SoftDelete = 4,
    Restore = 5,
    Login = 6,
    Logout = 7,
    PermissionChange = 8,
    WorkflowTransition = 9,
    MediaUpload = 10,
    MediaDelete = 11,
    PublishContent = 12,
    ArchiveContent = 13
}
