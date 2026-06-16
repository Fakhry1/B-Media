namespace BMedia.Application.Common.Constants;

/// <summary>Centralised permission name constants — single source of truth for all permission strings.</summary>
public static class PermissionNames
{
    // Contents
    public const string CreateContent    = "CreateContent";
    public const string UpdateContent    = "UpdateContent";
    public const string DeleteContent    = "DeleteContent";
    public const string PublishContent   = "PublishContent";
    public const string ReviewContent    = "ReviewContent";
    public const string ApproveContent   = "ApproveContent";

    // Media Assets
    public const string UploadMedia      = "UploadMedia";
    public const string DeleteMedia      = "DeleteMedia";
    public const string ViewSignedUrl    = "ViewSignedUrl";

    // Categories
    public const string ManageCategories = "ManageCategories";

    // Users & Roles
    public const string ManageUsers      = "ManageUsers";
    public const string ManageRoles      = "ManageRoles";
    public const string ViewAuditLogs    = "ViewAuditLogs";
}
