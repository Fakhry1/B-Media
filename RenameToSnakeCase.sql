-- ============================================================
-- BMedia — Rename PascalCase → snake_case
-- يعيد تسمية الجداول والأعمدة لتطابق EF Core UseSnakeCaseNamingConvention
-- شغّله مرة واحدة فقط على قاعدة البيانات الحالية
-- ============================================================

BEGIN;

-- ============================================================
-- 1. إعادة تسمية الجداول
-- ============================================================

ALTER TABLE "Roles"                    RENAME TO roles;
ALTER TABLE "Permissions"              RENAME TO permissions;
ALTER TABLE "Users"                    RENAME TO users;
ALTER TABLE "UserRoles"                RENAME TO user_roles;
ALTER TABLE "RolePermissions"          RENAME TO role_permissions;
ALTER TABLE "RefreshTokens"            RENAME TO refresh_tokens;
ALTER TABLE "Categories"               RENAME TO categories;
ALTER TABLE "Subcategories"            RENAME TO subcategories;
ALTER TABLE "Tags"                     RENAME TO tags;
ALTER TABLE "WorkflowDefinitions"      RENAME TO workflow_definitions;
ALTER TABLE "WorkflowSteps"            RENAME TO workflow_steps;
ALTER TABLE "WorkflowTransitions"      RENAME TO workflow_transitions;
ALTER TABLE "Contents"                 RENAME TO contents;
ALTER TABLE "ContentTags"              RENAME TO content_tags;
ALTER TABLE "MediaAssets"              RENAME TO media_assets;
ALTER TABLE "MediaVersions"            RENAME TO media_versions;
ALTER TABLE "Localizations"            RENAME TO localizations;
ALTER TABLE "Attachments"              RENAME TO attachments;
ALTER TABLE "ScheduledPublications"    RENAME TO scheduled_publications;
ALTER TABLE "ReviewComments"           RENAME TO review_comments;
ALTER TABLE "ContentWorkflowHistories" RENAME TO content_workflow_histories;
ALTER TABLE "Notifications"            RENAME TO notifications;
ALTER TABLE "AuditLogs"                RENAME TO audit_logs;

-- ============================================================
-- 2. أعمدة جدول roles
-- ============================================================
ALTER TABLE roles RENAME COLUMN "Id"             TO id;
ALTER TABLE roles RENAME COLUMN "Name"           TO name;
ALTER TABLE roles RENAME COLUMN "NormalizedName" TO normalized_name;
ALTER TABLE roles RENAME COLUMN "Description"    TO description;
ALTER TABLE roles RENAME COLUMN "IsSystem"       TO is_system;
ALTER TABLE roles RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE roles RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE roles RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE roles RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE roles RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE roles RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE roles RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 3. أعمدة جدول permissions
-- ============================================================
ALTER TABLE permissions RENAME COLUMN "Id"             TO id;
ALTER TABLE permissions RENAME COLUMN "Name"           TO name;
ALTER TABLE permissions RENAME COLUMN "NormalizedName" TO normalized_name;
ALTER TABLE permissions RENAME COLUMN "Description"    TO description;
ALTER TABLE permissions RENAME COLUMN "Module"         TO module;
ALTER TABLE permissions RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE permissions RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE permissions RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE permissions RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE permissions RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE permissions RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE permissions RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 4. أعمدة جدول users
-- ============================================================
ALTER TABLE users RENAME COLUMN "Id"                             TO id;
ALTER TABLE users RENAME COLUMN "Username"                       TO username;
ALTER TABLE users RENAME COLUMN "Email"                          TO email;
ALTER TABLE users RENAME COLUMN "PasswordHash"                   TO password_hash;
ALTER TABLE users RENAME COLUMN "FirstName"                      TO first_name;
ALTER TABLE users RENAME COLUMN "LastName"                       TO last_name;
ALTER TABLE users RENAME COLUMN "PhoneNumber"                    TO phone_number;
ALTER TABLE users RENAME COLUMN "ProfilePictureUrl"              TO profile_picture_url;
ALTER TABLE users RENAME COLUMN "IsActive"                       TO is_active;
ALTER TABLE users RENAME COLUMN "IsEmailVerified"                TO is_email_verified;
ALTER TABLE users RENAME COLUMN "LastLoginAt"                    TO last_login_at;
ALTER TABLE users RENAME COLUMN "LastLoginIp"                    TO last_login_ip;
ALTER TABLE users RENAME COLUMN "FailedLoginAttempts"            TO failed_login_attempts;
ALTER TABLE users RENAME COLUMN "LockoutEnd"                     TO lockout_end;
ALTER TABLE users RENAME COLUMN "EmailVerificationToken"         TO email_verification_token;
ALTER TABLE users RENAME COLUMN "EmailVerificationTokenExpiry"   TO email_verification_token_expiry;
ALTER TABLE users RENAME COLUMN "PasswordResetToken"             TO password_reset_token;
ALTER TABLE users RENAME COLUMN "PasswordResetTokenExpiry"       TO password_reset_token_expiry;
ALTER TABLE users RENAME COLUMN "PreferredLanguage"              TO preferred_language;
ALTER TABLE users RENAME COLUMN "TimeZone"                       TO time_zone;
ALTER TABLE users RENAME COLUMN "CreatedAt"                      TO created_at;
ALTER TABLE users RENAME COLUMN "CreatedBy"                      TO created_by;
ALTER TABLE users RENAME COLUMN "UpdatedAt"                      TO updated_at;
ALTER TABLE users RENAME COLUMN "UpdatedBy"                      TO updated_by;
ALTER TABLE users RENAME COLUMN "IsDeleted"                      TO is_deleted;
ALTER TABLE users RENAME COLUMN "DeletedAt"                      TO deleted_at;
ALTER TABLE users RENAME COLUMN "DeletedBy"                      TO deleted_by;

-- ============================================================
-- 5. أعمدة جدول user_roles
-- ============================================================
ALTER TABLE user_roles RENAME COLUMN "UserId"      TO user_id;
ALTER TABLE user_roles RENAME COLUMN "RoleId"      TO role_id;
ALTER TABLE user_roles RENAME COLUMN "AssignedAt"  TO assigned_at;
ALTER TABLE user_roles RENAME COLUMN "AssignedBy"  TO assigned_by;

-- ============================================================
-- 6. أعمدة جدول role_permissions
-- ============================================================
ALTER TABLE role_permissions RENAME COLUMN "RoleId"       TO role_id;
ALTER TABLE role_permissions RENAME COLUMN "PermissionId" TO permission_id;
ALTER TABLE role_permissions RENAME COLUMN "GrantedAt"    TO granted_at;
ALTER TABLE role_permissions RENAME COLUMN "GrantedBy"    TO granted_by;

-- ============================================================
-- 7. أعمدة جدول refresh_tokens
-- ============================================================
ALTER TABLE refresh_tokens RENAME COLUMN "Id"               TO id;
ALTER TABLE refresh_tokens RENAME COLUMN "UserId"           TO user_id;
ALTER TABLE refresh_tokens RENAME COLUMN "Token"            TO token;
ALTER TABLE refresh_tokens RENAME COLUMN "ExpiresAt"        TO expires_at;
ALTER TABLE refresh_tokens RENAME COLUMN "IsRevoked"        TO is_revoked;
ALTER TABLE refresh_tokens RENAME COLUMN "RevokedAt"        TO revoked_at;
ALTER TABLE refresh_tokens RENAME COLUMN "RevokedReason"    TO revoked_reason;
ALTER TABLE refresh_tokens RENAME COLUMN "ReplacedByToken"  TO replaced_by_token;
ALTER TABLE refresh_tokens RENAME COLUMN "CreatedByIp"      TO created_by_ip;
ALTER TABLE refresh_tokens RENAME COLUMN "RevokedByIp"      TO revoked_by_ip;
ALTER TABLE refresh_tokens RENAME COLUMN "CreatedAt"        TO created_at;
ALTER TABLE refresh_tokens RENAME COLUMN "CreatedBy"        TO created_by;
ALTER TABLE refresh_tokens RENAME COLUMN "UpdatedAt"        TO updated_at;
ALTER TABLE refresh_tokens RENAME COLUMN "UpdatedBy"        TO updated_by;
ALTER TABLE refresh_tokens RENAME COLUMN "IsDeleted"        TO is_deleted;
ALTER TABLE refresh_tokens RENAME COLUMN "DeletedAt"        TO deleted_at;
ALTER TABLE refresh_tokens RENAME COLUMN "DeletedBy"        TO deleted_by;
ALTER TABLE refresh_tokens RENAME COLUMN "RowVersion"       TO row_version;

-- ============================================================
-- 8. أعمدة جدول categories
-- ============================================================
ALTER TABLE categories RENAME COLUMN "Id"          TO id;
ALTER TABLE categories RENAME COLUMN "Name"        TO name;
ALTER TABLE categories RENAME COLUMN "Slug"        TO slug;
ALTER TABLE categories RENAME COLUMN "Description" TO description;
ALTER TABLE categories RENAME COLUMN "IconUrl"     TO icon_url;
ALTER TABLE categories RENAME COLUMN "SortOrder"   TO sort_order;
ALTER TABLE categories RENAME COLUMN "IsActive"    TO is_active;
ALTER TABLE categories RENAME COLUMN "CreatedAt"   TO created_at;
ALTER TABLE categories RENAME COLUMN "CreatedBy"   TO created_by;
ALTER TABLE categories RENAME COLUMN "UpdatedAt"   TO updated_at;
ALTER TABLE categories RENAME COLUMN "UpdatedBy"   TO updated_by;
ALTER TABLE categories RENAME COLUMN "IsDeleted"   TO is_deleted;
ALTER TABLE categories RENAME COLUMN "DeletedAt"   TO deleted_at;
ALTER TABLE categories RENAME COLUMN "DeletedBy"   TO deleted_by;

-- ============================================================
-- 9. أعمدة جدول subcategories
-- ============================================================
ALTER TABLE subcategories RENAME COLUMN "Id"          TO id;
ALTER TABLE subcategories RENAME COLUMN "Name"        TO name;
ALTER TABLE subcategories RENAME COLUMN "Slug"        TO slug;
ALTER TABLE subcategories RENAME COLUMN "Description" TO description;
ALTER TABLE subcategories RENAME COLUMN "SortOrder"   TO sort_order;
ALTER TABLE subcategories RENAME COLUMN "IsActive"    TO is_active;
ALTER TABLE subcategories RENAME COLUMN "CategoryId"  TO category_id;
ALTER TABLE subcategories RENAME COLUMN "CreatedAt"   TO created_at;
ALTER TABLE subcategories RENAME COLUMN "CreatedBy"   TO created_by;
ALTER TABLE subcategories RENAME COLUMN "UpdatedAt"   TO updated_at;
ALTER TABLE subcategories RENAME COLUMN "UpdatedBy"   TO updated_by;
ALTER TABLE subcategories RENAME COLUMN "IsDeleted"   TO is_deleted;
ALTER TABLE subcategories RENAME COLUMN "DeletedAt"   TO deleted_at;
ALTER TABLE subcategories RENAME COLUMN "DeletedBy"   TO deleted_by;

-- ============================================================
-- 10. أعمدة جدول tags
-- ============================================================
ALTER TABLE tags RENAME COLUMN "Id"             TO id;
ALTER TABLE tags RENAME COLUMN "Name"           TO name;
ALTER TABLE tags RENAME COLUMN "Slug"           TO slug;
ALTER TABLE tags RENAME COLUMN "Description"    TO description;
ALTER TABLE tags RENAME COLUMN "IsAiGenerated"  TO is_ai_generated;
ALTER TABLE tags RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE tags RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE tags RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE tags RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE tags RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE tags RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE tags RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 11. أعمدة جدول workflow_definitions
-- ============================================================
ALTER TABLE workflow_definitions RENAME COLUMN "Id"          TO id;
ALTER TABLE workflow_definitions RENAME COLUMN "Name"        TO name;
ALTER TABLE workflow_definitions RENAME COLUMN "Description" TO description;
ALTER TABLE workflow_definitions RENAME COLUMN "IsDefault"   TO is_default;
ALTER TABLE workflow_definitions RENAME COLUMN "IsActive"    TO is_active;
ALTER TABLE workflow_definitions RENAME COLUMN "Version"     TO version;
ALTER TABLE workflow_definitions RENAME COLUMN "CreatedAt"   TO created_at;
ALTER TABLE workflow_definitions RENAME COLUMN "CreatedBy"   TO created_by;
ALTER TABLE workflow_definitions RENAME COLUMN "UpdatedAt"   TO updated_at;
ALTER TABLE workflow_definitions RENAME COLUMN "UpdatedBy"   TO updated_by;
ALTER TABLE workflow_definitions RENAME COLUMN "IsDeleted"   TO is_deleted;
ALTER TABLE workflow_definitions RENAME COLUMN "DeletedAt"   TO deleted_at;
ALTER TABLE workflow_definitions RENAME COLUMN "DeletedBy"   TO deleted_by;

-- ============================================================
-- 12. أعمدة جدول workflow_steps
-- ============================================================
ALTER TABLE workflow_steps RENAME COLUMN "Id"                   TO id;
ALTER TABLE workflow_steps RENAME COLUMN "WorkflowDefinitionId" TO workflow_definition_id;
ALTER TABLE workflow_steps RENAME COLUMN "Name"                 TO name;
ALTER TABLE workflow_steps RENAME COLUMN "Description"          TO description;
ALTER TABLE workflow_steps RENAME COLUMN "MapsToStatus"         TO maps_to_status;
ALTER TABLE workflow_steps RENAME COLUMN "Order"                TO "order";
ALTER TABLE workflow_steps RENAME COLUMN "IsInitial"            TO is_initial;
ALTER TABLE workflow_steps RENAME COLUMN "IsFinal"              TO is_final;
ALTER TABLE workflow_steps RENAME COLUMN "RequiresReviewer"     TO requires_reviewer;
ALTER TABLE workflow_steps RENAME COLUMN "RequiredPermission"   TO required_permission;
ALTER TABLE workflow_steps RENAME COLUMN "SlaHours"             TO sla_hours;
ALTER TABLE workflow_steps RENAME COLUMN "CreatedAt"            TO created_at;
ALTER TABLE workflow_steps RENAME COLUMN "CreatedBy"            TO created_by;
ALTER TABLE workflow_steps RENAME COLUMN "UpdatedAt"            TO updated_at;
ALTER TABLE workflow_steps RENAME COLUMN "UpdatedBy"            TO updated_by;
ALTER TABLE workflow_steps RENAME COLUMN "IsDeleted"            TO is_deleted;
ALTER TABLE workflow_steps RENAME COLUMN "DeletedAt"            TO deleted_at;
ALTER TABLE workflow_steps RENAME COLUMN "DeletedBy"            TO deleted_by;

-- ============================================================
-- 13. أعمدة جدول workflow_transitions
-- ============================================================
ALTER TABLE workflow_transitions RENAME COLUMN "Id"                   TO id;
ALTER TABLE workflow_transitions RENAME COLUMN "WorkflowDefinitionId" TO workflow_definition_id;
ALTER TABLE workflow_transitions RENAME COLUMN "FromStepId"           TO from_step_id;
ALTER TABLE workflow_transitions RENAME COLUMN "ToStepId"             TO to_step_id;
ALTER TABLE workflow_transitions RENAME COLUMN "ActionName"           TO action_name;
ALTER TABLE workflow_transitions RENAME COLUMN "Description"          TO description;
ALTER TABLE workflow_transitions RENAME COLUMN "RequiredPermission"   TO required_permission;
ALTER TABLE workflow_transitions RENAME COLUMN "RequiresComment"      TO requires_comment;
ALTER TABLE workflow_transitions RENAME COLUMN "IsActive"             TO is_active;
ALTER TABLE workflow_transitions RENAME COLUMN "CreatedAt"            TO created_at;
ALTER TABLE workflow_transitions RENAME COLUMN "CreatedBy"            TO created_by;
ALTER TABLE workflow_transitions RENAME COLUMN "UpdatedAt"            TO updated_at;
ALTER TABLE workflow_transitions RENAME COLUMN "UpdatedBy"            TO updated_by;
ALTER TABLE workflow_transitions RENAME COLUMN "IsDeleted"            TO is_deleted;
ALTER TABLE workflow_transitions RENAME COLUMN "DeletedAt"            TO deleted_at;
ALTER TABLE workflow_transitions RENAME COLUMN "DeletedBy"            TO deleted_by;

-- ============================================================
-- 14. أعمدة جدول contents
-- ============================================================
ALTER TABLE contents RENAME COLUMN "Id"                     TO id;
ALTER TABLE contents RENAME COLUMN "Title"                  TO title;
ALTER TABLE contents RENAME COLUMN "Slug"                   TO slug;
ALTER TABLE contents RENAME COLUMN "Summary"                TO summary;
ALTER TABLE contents RENAME COLUMN "Body"                   TO body;
ALTER TABLE contents RENAME COLUMN "Language"               TO language;
ALTER TABLE contents RENAME COLUMN "Status"                 TO status;
ALTER TABLE contents RENAME COLUMN "IsFeatured"             TO is_featured;
ALTER TABLE contents RENAME COLUMN "AllowComments"          TO allow_comments;
ALTER TABLE contents RENAME COLUMN "ViewCount"              TO view_count;
ALTER TABLE contents RENAME COLUMN "SeoTitle"               TO seo_title;
ALTER TABLE contents RENAME COLUMN "SeoDescription"         TO seo_description;
ALTER TABLE contents RENAME COLUMN "SeoKeywords"            TO seo_keywords;
ALTER TABLE contents RENAME COLUMN "CanonicalUrl"           TO canonical_url;
ALTER TABLE contents RENAME COLUMN "PublishedAt"            TO published_at;
ALTER TABLE contents RENAME COLUMN "PublishedBy"            TO published_by;
ALTER TABLE contents RENAME COLUMN "ScheduledPublishAt"     TO scheduled_publish_at;
ALTER TABLE contents RENAME COLUMN "ArchivedAt"             TO archived_at;
ALTER TABLE contents RENAME COLUMN "CategoryId"             TO category_id;
ALTER TABLE contents RENAME COLUMN "SubcategoryId"          TO subcategory_id;
ALTER TABLE contents RENAME COLUMN "CurrentWorkflowStepId"  TO current_workflow_step_id;
ALTER TABLE contents RENAME COLUMN "AssignedReviewerId"     TO assigned_reviewer_id;
ALTER TABLE contents RENAME COLUMN "CreatedAt"              TO created_at;
ALTER TABLE contents RENAME COLUMN "CreatedBy"              TO created_by;
ALTER TABLE contents RENAME COLUMN "UpdatedAt"              TO updated_at;
ALTER TABLE contents RENAME COLUMN "UpdatedBy"              TO updated_by;
ALTER TABLE contents RENAME COLUMN "IsDeleted"              TO is_deleted;
ALTER TABLE contents RENAME COLUMN "DeletedAt"              TO deleted_at;
ALTER TABLE contents RENAME COLUMN "DeletedBy"              TO deleted_by;

-- ============================================================
-- 15. أعمدة جدول content_tags
-- ============================================================
ALTER TABLE content_tags RENAME COLUMN "ContentId"      TO content_id;
ALTER TABLE content_tags RENAME COLUMN "TagId"          TO tag_id;
ALTER TABLE content_tags RENAME COLUMN "TaggedAt"       TO tagged_at;
ALTER TABLE content_tags RENAME COLUMN "IsAiGenerated"  TO is_ai_generated;

-- ============================================================
-- 16. أعمدة جدول media_assets
-- ============================================================
ALTER TABLE media_assets RENAME COLUMN "Id"                     TO id;
ALTER TABLE media_assets RENAME COLUMN "OriginalFileName"        TO original_file_name;
ALTER TABLE media_assets RENAME COLUMN "StorageKey"              TO storage_key;
ALTER TABLE media_assets RENAME COLUMN "PublicUrl"               TO public_url;
ALTER TABLE media_assets RENAME COLUMN "CdnUrl"                  TO cdn_url;
ALTER TABLE media_assets RENAME COLUMN "ContentType"             TO content_type;
ALTER TABLE media_assets RENAME COLUMN "FileSizeBytes"           TO file_size_bytes;
ALTER TABLE media_assets RENAME COLUMN "MediaType"               TO media_type;
ALTER TABLE media_assets RENAME COLUMN "Status"                  TO status;
ALTER TABLE media_assets RENAME COLUMN "StorageProvider"         TO storage_provider;
ALTER TABLE media_assets RENAME COLUMN "Title"                   TO title;
ALTER TABLE media_assets RENAME COLUMN "Description"             TO description;
ALTER TABLE media_assets RENAME COLUMN "AltText"                 TO alt_text;
ALTER TABLE media_assets RENAME COLUMN "Language"                TO language;
ALTER TABLE media_assets RENAME COLUMN "DurationSeconds"         TO duration_seconds;
ALTER TABLE media_assets RENAME COLUMN "Width"                   TO width;
ALTER TABLE media_assets RENAME COLUMN "Height"                  TO height;
ALTER TABLE media_assets RENAME COLUMN "AspectRatio"             TO aspect_ratio;
ALTER TABLE media_assets RENAME COLUMN "Codec"                   TO codec;
ALTER TABLE media_assets RENAME COLUMN "Bitrate"                 TO bitrate;
ALTER TABLE media_assets RENAME COLUMN "FrameRate"               TO frame_rate;
ALTER TABLE media_assets RENAME COLUMN "ThumbnailUrl"            TO thumbnail_url;
ALTER TABLE media_assets RENAME COLUMN "PreviewUrl"              TO preview_url;
ALTER TABLE media_assets RENAME COLUMN "HlsManifestUrl"          TO hls_manifest_url;
ALTER TABLE media_assets RENAME COLUMN "IsTranscodingComplete"   TO is_transcoding_complete;
ALTER TABLE media_assets RENAME COLUMN "IsThumbnailGenerated"    TO is_thumbnail_generated;
ALTER TABLE media_assets RENAME COLUMN "IsMetadataExtracted"     TO is_metadata_extracted;
ALTER TABLE media_assets RENAME COLUMN "AntivirusScanPassed"     TO antivirus_scan_passed;
ALTER TABLE media_assets RENAME COLUMN "AntivirusScannedAt"      TO antivirus_scanned_at;
ALTER TABLE media_assets RENAME COLUMN "ExtractedMetadata"       TO extracted_metadata;
ALTER TABLE media_assets RENAME COLUMN "AiGeneratedTags"         TO ai_generated_tags;
ALTER TABLE media_assets RENAME COLUMN "OcrText"                 TO ocr_text;
ALTER TABLE media_assets RENAME COLUMN "IsWatermarked"           TO is_watermarked;
ALTER TABLE media_assets RENAME COLUMN "SortOrder"               TO sort_order;
ALTER TABLE media_assets RENAME COLUMN "IsPrimary"               TO is_primary;
ALTER TABLE media_assets RENAME COLUMN "ContentId"               TO content_id;
ALTER TABLE media_assets RENAME COLUMN "CreatedAt"               TO created_at;
ALTER TABLE media_assets RENAME COLUMN "CreatedBy"               TO created_by;
ALTER TABLE media_assets RENAME COLUMN "UpdatedAt"               TO updated_at;
ALTER TABLE media_assets RENAME COLUMN "UpdatedBy"               TO updated_by;
ALTER TABLE media_assets RENAME COLUMN "IsDeleted"               TO is_deleted;
ALTER TABLE media_assets RENAME COLUMN "DeletedAt"               TO deleted_at;
ALTER TABLE media_assets RENAME COLUMN "DeletedBy"               TO deleted_by;

-- ============================================================
-- 17. أعمدة جدول media_versions
-- ============================================================
ALTER TABLE media_versions RENAME COLUMN "Id"              TO id;
ALTER TABLE media_versions RENAME COLUMN "MediaAssetId"    TO media_asset_id;
ALTER TABLE media_versions RENAME COLUMN "StorageKey"      TO storage_key;
ALTER TABLE media_versions RENAME COLUMN "PublicUrl"       TO public_url;
ALTER TABLE media_versions RENAME COLUMN "CdnUrl"          TO cdn_url;
ALTER TABLE media_versions RENAME COLUMN "ContentType"     TO content_type;
ALTER TABLE media_versions RENAME COLUMN "FileSizeBytes"   TO file_size_bytes;
ALTER TABLE media_versions RENAME COLUMN "Width"           TO width;
ALTER TABLE media_versions RENAME COLUMN "Height"          TO height;
ALTER TABLE media_versions RENAME COLUMN "Bitrate"         TO bitrate;
ALTER TABLE media_versions RENAME COLUMN "DurationSeconds" TO duration_seconds;
ALTER TABLE media_versions RENAME COLUMN "Quality"         TO quality;
ALTER TABLE media_versions RENAME COLUMN "VersionLabel"    TO version_label;
ALTER TABLE media_versions RENAME COLUMN "IsHls"           TO is_hls;
ALTER TABLE media_versions RENAME COLUMN "HlsManifestUrl"  TO hls_manifest_url;
ALTER TABLE media_versions RENAME COLUMN "IsDefault"       TO is_default;
ALTER TABLE media_versions RENAME COLUMN "StorageProvider" TO storage_provider;
ALTER TABLE media_versions RENAME COLUMN "CreatedAt"       TO created_at;
ALTER TABLE media_versions RENAME COLUMN "CreatedBy"       TO created_by;
ALTER TABLE media_versions RENAME COLUMN "UpdatedAt"       TO updated_at;
ALTER TABLE media_versions RENAME COLUMN "UpdatedBy"       TO updated_by;
ALTER TABLE media_versions RENAME COLUMN "IsDeleted"       TO is_deleted;
ALTER TABLE media_versions RENAME COLUMN "DeletedAt"       TO deleted_at;
ALTER TABLE media_versions RENAME COLUMN "DeletedBy"       TO deleted_by;

-- ============================================================
-- 18. أعمدة جدول localizations
-- ============================================================
ALTER TABLE localizations RENAME COLUMN "Id"             TO id;
ALTER TABLE localizations RENAME COLUMN "ContentId"      TO content_id;
ALTER TABLE localizations RENAME COLUMN "Language"       TO language;
ALTER TABLE localizations RENAME COLUMN "Title"          TO title;
ALTER TABLE localizations RENAME COLUMN "Summary"        TO summary;
ALTER TABLE localizations RENAME COLUMN "Body"           TO body;
ALTER TABLE localizations RENAME COLUMN "SeoTitle"       TO seo_title;
ALTER TABLE localizations RENAME COLUMN "SeoDescription" TO seo_description;
ALTER TABLE localizations RENAME COLUMN "IsApproved"     TO is_approved;
ALTER TABLE localizations RENAME COLUMN "ApprovedAt"     TO approved_at;
ALTER TABLE localizations RENAME COLUMN "ApprovedBy"     TO approved_by;
ALTER TABLE localizations RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE localizations RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE localizations RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE localizations RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE localizations RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE localizations RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE localizations RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 19. أعمدة جدول attachments
-- ============================================================
ALTER TABLE attachments RENAME COLUMN "Id"             TO id;
ALTER TABLE attachments RENAME COLUMN "ContentId"      TO content_id;
ALTER TABLE attachments RENAME COLUMN "FileName"       TO file_name;
ALTER TABLE attachments RENAME COLUMN "StorageKey"     TO storage_key;
ALTER TABLE attachments RENAME COLUMN "PublicUrl"      TO public_url;
ALTER TABLE attachments RENAME COLUMN "ContentType"    TO content_type;
ALTER TABLE attachments RENAME COLUMN "FileSizeBytes"  TO file_size_bytes;
ALTER TABLE attachments RENAME COLUMN "Description"    TO description;
ALTER TABLE attachments RENAME COLUMN "SortOrder"      TO sort_order;
ALTER TABLE attachments RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE attachments RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE attachments RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE attachments RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE attachments RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE attachments RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE attachments RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 20. أعمدة جدول scheduled_publications
-- ============================================================
ALTER TABLE scheduled_publications RENAME COLUMN "Id"             TO id;
ALTER TABLE scheduled_publications RENAME COLUMN "ContentId"      TO content_id;
ALTER TABLE scheduled_publications RENAME COLUMN "ScheduledAt"    TO scheduled_at;
ALTER TABLE scheduled_publications RENAME COLUMN "IsExecuted"     TO is_executed;
ALTER TABLE scheduled_publications RENAME COLUMN "ExecutedAt"     TO executed_at;
ALTER TABLE scheduled_publications RENAME COLUMN "IsSuccessful"   TO is_successful;
ALTER TABLE scheduled_publications RENAME COLUMN "ErrorMessage"   TO error_message;
ALTER TABLE scheduled_publications RENAME COLUMN "HangfireJobId"  TO hangfire_job_id;
ALTER TABLE scheduled_publications RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE scheduled_publications RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE scheduled_publications RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE scheduled_publications RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE scheduled_publications RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE scheduled_publications RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE scheduled_publications RENAME COLUMN "DeletedBy"      TO deleted_by;

-- ============================================================
-- 21. أعمدة جدول review_comments
-- ============================================================
ALTER TABLE review_comments RENAME COLUMN "Id"              TO id;
ALTER TABLE review_comments RENAME COLUMN "ContentId"       TO content_id;
ALTER TABLE review_comments RENAME COLUMN "ReviewerId"      TO reviewer_id;
ALTER TABLE review_comments RENAME COLUMN "Comment"         TO comment;
ALTER TABLE review_comments RENAME COLUMN "IsResolved"      TO is_resolved;
ALTER TABLE review_comments RENAME COLUMN "ResolvedAt"      TO resolved_at;
ALTER TABLE review_comments RENAME COLUMN "ResolvedBy"      TO resolved_by;
ALTER TABLE review_comments RENAME COLUMN "ParentCommentId" TO parent_comment_id;
ALTER TABLE review_comments RENAME COLUMN "CreatedAt"       TO created_at;
ALTER TABLE review_comments RENAME COLUMN "CreatedBy"       TO created_by;
ALTER TABLE review_comments RENAME COLUMN "UpdatedAt"       TO updated_at;
ALTER TABLE review_comments RENAME COLUMN "UpdatedBy"       TO updated_by;
ALTER TABLE review_comments RENAME COLUMN "IsDeleted"       TO is_deleted;
ALTER TABLE review_comments RENAME COLUMN "DeletedAt"       TO deleted_at;
ALTER TABLE review_comments RENAME COLUMN "DeletedBy"       TO deleted_by;

-- ============================================================
-- 22. أعمدة جدول content_workflow_histories
-- ============================================================
ALTER TABLE content_workflow_histories RENAME COLUMN "Id"                TO id;
ALTER TABLE content_workflow_histories RENAME COLUMN "ContentId"         TO content_id;
ALTER TABLE content_workflow_histories RENAME COLUMN "FromStepId"        TO from_step_id;
ALTER TABLE content_workflow_histories RENAME COLUMN "ToStepId"          TO to_step_id;
ALTER TABLE content_workflow_histories RENAME COLUMN "TransitionedById"  TO transitioned_by_id;
ALTER TABLE content_workflow_histories RENAME COLUMN "FromStatus"        TO from_status;
ALTER TABLE content_workflow_histories RENAME COLUMN "ToStatus"          TO to_status;
ALTER TABLE content_workflow_histories RENAME COLUMN "Comment"           TO comment;
ALTER TABLE content_workflow_histories RENAME COLUMN "ActionName"        TO action_name;
ALTER TABLE content_workflow_histories RENAME COLUMN "TransitionedAt"    TO transitioned_at;
ALTER TABLE content_workflow_histories RENAME COLUMN "CreatedAt"         TO created_at;
ALTER TABLE content_workflow_histories RENAME COLUMN "CreatedBy"         TO created_by;
ALTER TABLE content_workflow_histories RENAME COLUMN "UpdatedAt"         TO updated_at;
ALTER TABLE content_workflow_histories RENAME COLUMN "UpdatedBy"         TO updated_by;
ALTER TABLE content_workflow_histories RENAME COLUMN "IsDeleted"         TO is_deleted;
ALTER TABLE content_workflow_histories RENAME COLUMN "DeletedAt"         TO deleted_at;
ALTER TABLE content_workflow_histories RENAME COLUMN "DeletedBy"         TO deleted_by;
ALTER TABLE content_workflow_histories RENAME COLUMN "RowVersion"        TO row_version;

-- ============================================================
-- 23. أعمدة جدول notifications
-- ============================================================
ALTER TABLE notifications RENAME COLUMN "Id"            TO id;
ALTER TABLE notifications RENAME COLUMN "UserId"        TO user_id;
ALTER TABLE notifications RENAME COLUMN "Type"          TO type;
ALTER TABLE notifications RENAME COLUMN "Title"         TO title;
ALTER TABLE notifications RENAME COLUMN "Message"       TO message;
ALTER TABLE notifications RENAME COLUMN "IsRead"        TO is_read;
ALTER TABLE notifications RENAME COLUMN "ReadAt"        TO read_at;
ALTER TABLE notifications RENAME COLUMN "ReferenceId"   TO reference_id;
ALTER TABLE notifications RENAME COLUMN "ReferenceType" TO reference_type;
ALTER TABLE notifications RENAME COLUMN "ActionUrl"     TO action_url;
ALTER TABLE notifications RENAME COLUMN "Metadata"      TO metadata;
ALTER TABLE notifications RENAME COLUMN "CreatedAt"     TO created_at;
ALTER TABLE notifications RENAME COLUMN "CreatedBy"     TO created_by;
ALTER TABLE notifications RENAME COLUMN "UpdatedAt"     TO updated_at;
ALTER TABLE notifications RENAME COLUMN "UpdatedBy"     TO updated_by;
ALTER TABLE notifications RENAME COLUMN "IsDeleted"     TO is_deleted;
ALTER TABLE notifications RENAME COLUMN "DeletedAt"     TO deleted_at;
ALTER TABLE notifications RENAME COLUMN "DeletedBy"     TO deleted_by;

-- ============================================================
-- 24. أعمدة جدول audit_logs
-- ============================================================
ALTER TABLE audit_logs RENAME COLUMN "Id"             TO id;
ALTER TABLE audit_logs RENAME COLUMN "UserId"         TO user_id;
ALTER TABLE audit_logs RENAME COLUMN "UserEmail"      TO user_email;
ALTER TABLE audit_logs RENAME COLUMN "Action"         TO action;
ALTER TABLE audit_logs RENAME COLUMN "EntityType"     TO entity_type;
ALTER TABLE audit_logs RENAME COLUMN "EntityId"       TO entity_id;
ALTER TABLE audit_logs RENAME COLUMN "OldValues"      TO old_values;
ALTER TABLE audit_logs RENAME COLUMN "NewValues"      TO new_values;
ALTER TABLE audit_logs RENAME COLUMN "IpAddress"      TO ip_address;
ALTER TABLE audit_logs RENAME COLUMN "UserAgent"      TO user_agent;
ALTER TABLE audit_logs RENAME COLUMN "AdditionalData" TO additional_data;
ALTER TABLE audit_logs RENAME COLUMN "IsSuccessful"   TO is_successful;
ALTER TABLE audit_logs RENAME COLUMN "ErrorMessage"   TO error_message;
ALTER TABLE audit_logs RENAME COLUMN "CreatedAt"      TO created_at;
ALTER TABLE audit_logs RENAME COLUMN "CreatedBy"      TO created_by;
ALTER TABLE audit_logs RENAME COLUMN "UpdatedAt"      TO updated_at;
ALTER TABLE audit_logs RENAME COLUMN "UpdatedBy"      TO updated_by;
ALTER TABLE audit_logs RENAME COLUMN "IsDeleted"      TO is_deleted;
ALTER TABLE audit_logs RENAME COLUMN "DeletedAt"      TO deleted_at;
ALTER TABLE audit_logs RENAME COLUMN "DeletedBy"      TO deleted_by;
ALTER TABLE audit_logs RENAME COLUMN "RowVersion"     TO row_version;

COMMIT;

-- تحقق من الجداول بعد التعديل
SELECT table_name
FROM   information_schema.tables
WHERE  table_schema = 'public'
ORDER  BY table_name;
