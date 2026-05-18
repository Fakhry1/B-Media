using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace BMedia.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===================== ROLES =====================
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_roles", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_roles_normalized_name", table: "Roles", column: "normalized_name", unique: true, filter: "\"IsDeleted\" = false");

            // ===================== PERMISSIONS =====================
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    module = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_permissions", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_permissions_normalized_name", table: "Permissions", column: "normalized_name", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_permissions_module", table: "Permissions", column: "module");

            // ===================== USERS =====================
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    profile_picture_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_login_ip = table.Column<string>(type: "text", nullable: true),
                    failed_login_attempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    lockout_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email_verification_token = table.Column<string>(type: "text", nullable: true),
                    email_verification_token_expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    password_reset_token = table.Column<string>(type: "text", nullable: true),
                    password_reset_token_expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    preferred_language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    time_zone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_users", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_users_email", table: "Users", column: "email", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_users_username", table: "Users", column: "username", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_users_is_deleted", table: "Users", column: "is_deleted");
            migrationBuilder.CreateIndex(name: "ix_users_is_active", table: "Users", column: "is_active");

            // ===================== USER ROLES =====================
            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    assigned_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(name: "fk_user_roles_users_user_id", column: x => x.user_id, principalTable: "Users", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_user_roles_roles_role_id", column: x => x.role_id, principalTable: "Roles", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_user_roles_user_id", table: "UserRoles", column: "user_id");
            migrationBuilder.CreateIndex(name: "ix_user_roles_role_id", table: "UserRoles", column: "role_id");

            // ===================== ROLE PERMISSIONS =====================
            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    granted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    granted_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(name: "fk_role_permissions_roles_role_id", column: x => x.role_id, principalTable: "Roles", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_role_permissions_permissions_permission_id", column: x => x.permission_id, principalTable: "Permissions", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            // ===================== REFRESH TOKENS =====================
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    revoked_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    replaced_by_token = table.Column<string>(type: "text", nullable: true),
                    created_by_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    revoked_by_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(name: "fk_refresh_tokens_users_user_id", column: x => x.user_id, principalTable: "Users", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_refresh_tokens_token", table: "RefreshTokens", column: "token", unique: true);
            migrationBuilder.CreateIndex(name: "ix_refresh_tokens_user_id", table: "RefreshTokens", column: "user_id");
            migrationBuilder.CreateIndex(name: "ix_refresh_tokens_expires_at", table: "RefreshTokens", column: "expires_at");

            // ===================== CATEGORIES =====================
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    icon_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_categories", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_categories_slug", table: "Categories", column: "slug", unique: true, filter: "\"IsDeleted\" = false");

            // ===================== SUBCATEGORIES =====================
            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subcategories", x => x.id);
                    table.ForeignKey(name: "fk_subcategories_categories_category_id", column: x => x.category_id, principalTable: "Categories", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_subcategories_slug", table: "Subcategories", column: "slug", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_subcategories_category_id", table: "Subcategories", column: "category_id");

            // ===================== TAGS =====================
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_ai_generated = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_tags", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_tags_slug", table: "Tags", column: "slug", unique: true, filter: "\"IsDeleted\" = false");

            // ===================== WORKFLOW DEFINITIONS =====================
            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_workflow_definitions", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_workflow_definitions_is_default", table: "WorkflowDefinitions", column: "is_default");

            // ===================== WORKFLOW STEPS =====================
            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    workflow_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    maps_to_status = table.Column<int>(type: "integer", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_initial = table.Column<bool>(type: "boolean", nullable: false),
                    is_final = table.Column<bool>(type: "boolean", nullable: false),
                    requires_reviewer = table.Column<bool>(type: "boolean", nullable: false),
                    required_permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sla_hours = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_steps", x => x.id);
                    table.ForeignKey(name: "fk_workflow_steps_workflow_definitions_workflow_definition_id", column: x => x.workflow_definition_id, principalTable: "WorkflowDefinitions", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_workflow_steps_workflow_definition_id", table: "WorkflowSteps", column: "workflow_definition_id");
            migrationBuilder.CreateIndex(name: "ix_workflow_steps_workflow_definition_id_order", table: "WorkflowSteps", columns: new[] { "workflow_definition_id", "order" });

            // ===================== WORKFLOW TRANSITIONS =====================
            migrationBuilder.CreateTable(
                name: "WorkflowTransitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    workflow_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    required_permission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    requires_comment = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_transitions", x => x.id);
                    table.ForeignKey(name: "fk_workflow_transitions_workflow_definitions_workflow_definition_id", column: x => x.workflow_definition_id, principalTable: "WorkflowDefinitions", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_workflow_transitions_workflow_steps_from_step_id", column: x => x.from_step_id, principalTable: "WorkflowSteps", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(name: "fk_workflow_transitions_workflow_steps_to_step_id", column: x => x.to_step_id, principalTable: "WorkflowSteps", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(name: "ix_workflow_transitions_from_step_id_to_step_id", table: "WorkflowTransitions", columns: new[] { "from_step_id", "to_step_id" }, unique: true);
            migrationBuilder.CreateIndex(name: "ix_workflow_transitions_workflow_definition_id", table: "WorkflowTransitions", column: "workflow_definition_id");

            // ===================== CONTENTS =====================
            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    slug = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false),
                    allow_comments = table.Column<bool>(type: "boolean", nullable: false),
                    view_count = table.Column<int>(type: "integer", nullable: false),
                    seo_title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    seo_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    seo_keywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    published_by = table.Column<Guid>(type: "uuid", nullable: true),
                    scheduled_publish_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    archived_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subcategory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    current_workflow_step_id = table.Column<Guid>(type: "uuid", nullable: true),
                    assigned_reviewer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contents", x => x.id);
                    table.ForeignKey(name: "fk_contents_categories_category_id", column: x => x.category_id, principalTable: "Categories", principalColumn: "id", onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(name: "fk_contents_subcategories_subcategory_id", column: x => x.subcategory_id, principalTable: "Subcategories", principalColumn: "id", onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(name: "fk_contents_workflow_steps_current_workflow_step_id", column: x => x.current_workflow_step_id, principalTable: "WorkflowSteps", principalColumn: "id", onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(name: "ix_contents_slug", table: "Contents", column: "slug", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_contents_status", table: "Contents", column: "status");
            migrationBuilder.CreateIndex(name: "ix_contents_language", table: "Contents", column: "language");
            migrationBuilder.CreateIndex(name: "ix_contents_is_featured", table: "Contents", column: "is_featured");
            migrationBuilder.CreateIndex(name: "ix_contents_published_at", table: "Contents", column: "published_at");
            migrationBuilder.CreateIndex(name: "ix_contents_category_id", table: "Contents", column: "category_id");
            migrationBuilder.CreateIndex(name: "ix_contents_created_at", table: "Contents", column: "created_at");
            migrationBuilder.CreateIndex(name: "ix_contents_status_language_is_deleted", table: "Contents", columns: new[] { "status", "language", "is_deleted" });

            // ===================== CONTENT TAGS =====================
            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tagged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_ai_generated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_tags", x => new { x.content_id, x.tag_id });
                    table.ForeignKey(name: "fk_content_tags_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_content_tags_tags_tag_id", column: x => x.tag_id, principalTable: "Tags", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            // ===================== MEDIA ASSETS =====================
            migrationBuilder.CreateTable(
                name: "MediaAssets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    original_file_name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    storage_key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    public_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    cdn_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    content_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    media_type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    storage_provider = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    alt_text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    aspect_ratio = table.Column<double>(type: "double precision", nullable: true),
                    codec = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bitrate = table.Column<int>(type: "integer", nullable: true),
                    frame_rate = table.Column<double>(type: "double precision", nullable: true),
                    thumbnail_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    preview_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    hls_manifest_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_transcoding_complete = table.Column<bool>(type: "boolean", nullable: false),
                    is_thumbnail_generated = table.Column<bool>(type: "boolean", nullable: false),
                    is_metadata_extracted = table.Column<bool>(type: "boolean", nullable: false),
                    antivirus_scan_passed = table.Column<bool>(type: "boolean", nullable: false),
                    antivirus_scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    extracted_metadata = table.Column<string>(type: "jsonb", nullable: true),
                    ai_generated_tags = table.Column<string>(type: "jsonb", nullable: true),
                    ocr_text = table.Column<string>(type: "text", nullable: true),
                    is_watermarked = table.Column<bool>(type: "boolean", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_assets", x => x.id);
                    table.ForeignKey(name: "fk_media_assets_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(name: "ix_media_assets_content_id", table: "MediaAssets", column: "content_id");
            migrationBuilder.CreateIndex(name: "ix_media_assets_media_type", table: "MediaAssets", column: "media_type");
            migrationBuilder.CreateIndex(name: "ix_media_assets_status", table: "MediaAssets", column: "status");
            migrationBuilder.CreateIndex(name: "ix_media_assets_storage_key", table: "MediaAssets", column: "storage_key", unique: true, filter: "\"IsDeleted\" = false");
            migrationBuilder.CreateIndex(name: "ix_media_assets_content_id_is_primary", table: "MediaAssets", columns: new[] { "content_id", "is_primary" });

            // ===================== MEDIA VERSIONS =====================
            migrationBuilder.CreateTable(
                name: "MediaVersions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    media_asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    storage_key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    public_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    cdn_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    content_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    bitrate = table.Column<int>(type: "integer", nullable: true),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    quality = table.Column<int>(type: "integer", nullable: true),
                    version_label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_hls = table.Column<bool>(type: "boolean", nullable: false),
                    hls_manifest_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    storage_provider = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_versions", x => x.id);
                    table.ForeignKey(name: "fk_media_versions_media_assets_media_asset_id", column: x => x.media_asset_id, principalTable: "MediaAssets", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_media_versions_media_asset_id", table: "MediaVersions", column: "media_asset_id");
            migrationBuilder.CreateIndex(name: "ix_media_versions_quality", table: "MediaVersions", column: "quality");
            migrationBuilder.CreateIndex(name: "ix_media_versions_is_default", table: "MediaVersions", column: "is_default");

            // ===================== LOCALIZATIONS =====================
            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    seo_title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    seo_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localizations", x => x.id);
                    table.ForeignKey(name: "fk_localizations_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_localizations_content_id_language", table: "Localizations", columns: new[] { "content_id", "language" }, unique: true, filter: "\"IsDeleted\" = false");

            // ===================== ATTACHMENTS =====================
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    storage_key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    public_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    content_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachments", x => x.id);
                    table.ForeignKey(name: "fk_attachments_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_attachments_content_id", table: "Attachments", column: "content_id");

            // ===================== SCHEDULED PUBLICATIONS =====================
            migrationBuilder.CreateTable(
                name: "ScheduledPublications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_executed = table.Column<bool>(type: "boolean", nullable: false),
                    executed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_successful = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    hangfire_job_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scheduled_publications", x => x.id);
                    table.ForeignKey(name: "fk_scheduled_publications_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_scheduled_publications_content_id", table: "ScheduledPublications", column: "content_id");
            migrationBuilder.CreateIndex(name: "ix_scheduled_publications_scheduled_at_is_executed", table: "ScheduledPublications", columns: new[] { "scheduled_at", "is_executed" });

            // ===================== REVIEW COMMENTS =====================
            migrationBuilder.CreateTable(
                name: "ReviewComments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    is_resolved = table.Column<bool>(type: "boolean", nullable: false),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    resolved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    parent_comment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review_comments", x => x.id);
                    table.ForeignKey(name: "fk_review_comments_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_review_comments_users_reviewer_id", column: x => x.reviewer_id, principalTable: "Users", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(name: "fk_review_comments_review_comments_parent_comment_id", column: x => x.parent_comment_id, principalTable: "ReviewComments", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(name: "ix_review_comments_content_id", table: "ReviewComments", column: "content_id");
            migrationBuilder.CreateIndex(name: "ix_review_comments_reviewer_id", table: "ReviewComments", column: "reviewer_id");

            // ===================== CONTENT WORKFLOW HISTORIES =====================
            migrationBuilder.CreateTable(
                name: "ContentWorkflowHistories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_step_id = table.Column<Guid>(type: "uuid", nullable: true),
                    to_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    transitioned_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_status = table.Column<int>(type: "integer", nullable: false),
                    to_status = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    action_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    transitioned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_workflow_histories", x => x.id);
                    table.ForeignKey(name: "fk_content_workflow_histories_contents_content_id", column: x => x.content_id, principalTable: "Contents", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "fk_content_workflow_histories_users_transitioned_by_id", column: x => x.transitioned_by_id, principalTable: "Users", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(name: "fk_content_workflow_histories_workflow_steps_from_step_id", column: x => x.from_step_id, principalTable: "WorkflowSteps", principalColumn: "id", onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(name: "fk_content_workflow_histories_workflow_steps_to_step_id", column: x => x.to_step_id, principalTable: "WorkflowSteps", principalColumn: "id", onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(name: "ix_content_workflow_histories_content_id", table: "ContentWorkflowHistories", column: "content_id");
            migrationBuilder.CreateIndex(name: "ix_content_workflow_histories_transitioned_at", table: "ContentWorkflowHistories", column: "transitioned_at");

            // ===================== NOTIFICATIONS =====================
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reference_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reference_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    action_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    metadata = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(name: "fk_notifications_users_user_id", column: x => x.user_id, principalTable: "Users", principalColumn: "id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "ix_notifications_user_id", table: "Notifications", column: "user_id");
            migrationBuilder.CreateIndex(name: "ix_notifications_user_id_is_read", table: "Notifications", columns: new[] { "user_id", "is_read" });
            migrationBuilder.CreateIndex(name: "ix_notifications_created_at", table: "Notifications", column: "created_at");

            // ===================== AUDIT LOGS =====================
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    action = table.Column<int>(type: "integer", nullable: false),
                    entity_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    entity_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    old_values = table.Column<string>(type: "jsonb", nullable: true),
                    new_values = table.Column<string>(type: "jsonb", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    additional_data = table.Column<string>(type: "jsonb", nullable: true),
                    is_successful = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    row_version = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table => table.PrimaryKey("pk_audit_logs", x => x.id));

            migrationBuilder.CreateIndex(name: "ix_audit_logs_user_id", table: "AuditLogs", column: "user_id");
            migrationBuilder.CreateIndex(name: "ix_audit_logs_action", table: "AuditLogs", column: "action");
            migrationBuilder.CreateIndex(name: "ix_audit_logs_entity_type", table: "AuditLogs", column: "entity_type");
            migrationBuilder.CreateIndex(name: "ix_audit_logs_created_at", table: "AuditLogs", column: "created_at");
            migrationBuilder.CreateIndex(name: "ix_audit_logs_entity_type_entity_id", table: "AuditLogs", columns: new[] { "entity_type", "entity_id" });

            // ===================== SEED DATA =====================
            var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id", "name", "normalized_name", "description", "is_system", "created_at", "is_deleted" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Administrator", "ADMINISTRATOR", "Full system access", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "ContentCreator", "CONTENTCREATOR", "Create and edit content", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Reviewer", "REVIEWER", "Review and approve content", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "LanguageReviewer", "LANGUAGEREVIEWER", "Review language and translations", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Designer", "DESIGNER", "Manage media assets", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Publisher", "PUBLISHER", "Publish and schedule content", true, now, false },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Archivist", "ARCHIVIST", "Archive and restore content", true, now, false }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "id", "name", "normalized_name", "description", "module", "created_at", "is_deleted" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "CreateContent", "CREATECONTENT", "CreateContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "EditContent", "EDITCONTENT", "EditContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "DeleteContent", "DELETECONTENT", "DeleteContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "PublishContent", "PUBLISHCONTENT", "PublishContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "ArchiveContent", "ARCHIVECONTENT", "ArchiveContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "ViewContent", "VIEWCONTENT", "ViewContent permission", "Content", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "UploadMedia", "UPLOADMEDIA", "UploadMedia permission", "Media", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "DeleteMedia", "DELETEMEDIA", "DeleteMedia permission", "Media", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000009"), "ManageMedia", "MANAGEMEDIA", "ManageMedia permission", "Media", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000010"), "ApproveReview", "APPROVEREVIEW", "ApproveReview permission", "Workflow", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000011"), "RejectReview", "REJECTREVIEW", "RejectReview permission", "Workflow", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000012"), "TransitionWorkflow", "TRANSITIONWORKFLOW", "TransitionWorkflow permission", "Workflow", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000013"), "ManageUsers", "MANAGEUSERS", "ManageUsers permission", "Admin", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000014"), "ManageRoles", "MANAGEROLES", "ManageRoles permission", "Admin", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000015"), "ViewAuditLogs", "VIEWAUDITLOGS", "ViewAuditLogs permission", "Admin", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000016"), "ManageCategories", "MANAGECATEGORIES", "ManageCategories permission", "Taxonomy", now, false },
                    { new Guid("20000000-0000-0000-0000-000000000017"), "ManageTags", "MANAGETAGS", "ManageTags permission", "Taxonomy", now, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AuditLogs");
            migrationBuilder.DropTable(name: "ContentWorkflowHistories");
            migrationBuilder.DropTable(name: "ContentTags");
            migrationBuilder.DropTable(name: "ReviewComments");
            migrationBuilder.DropTable(name: "Notifications");
            migrationBuilder.DropTable(name: "ScheduledPublications");
            migrationBuilder.DropTable(name: "Attachments");
            migrationBuilder.DropTable(name: "Localizations");
            migrationBuilder.DropTable(name: "MediaVersions");
            migrationBuilder.DropTable(name: "MediaAssets");
            migrationBuilder.DropTable(name: "Contents");
            migrationBuilder.DropTable(name: "WorkflowTransitions");
            migrationBuilder.DropTable(name: "WorkflowSteps");
            migrationBuilder.DropTable(name: "WorkflowDefinitions");
            migrationBuilder.DropTable(name: "Tags");
            migrationBuilder.DropTable(name: "Subcategories");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "RefreshTokens");
            migrationBuilder.DropTable(name: "UserRoles");
            migrationBuilder.DropTable(name: "RolePermissions");
            migrationBuilder.DropTable(name: "Users");
            migrationBuilder.DropTable(name: "Roles");
            migrationBuilder.DropTable(name: "Permissions");
        }
    }
}
