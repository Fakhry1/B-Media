using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(w => w.Name).IsRequired().HasMaxLength(200);
        builder.Property(w => w.Description).HasMaxLength(1000);
        builder.Property(w => w.RowVersion).IsRowVersion();
        builder.HasIndex(w => w.IsDefault);
    }
}

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.MapsToStatus).HasConversion<int>();
        builder.Property(s => s.RequiredPermission).HasMaxLength(200);
        builder.Property(s => s.RowVersion).IsRowVersion();

        builder.HasOne(s => s.WorkflowDefinition).WithMany(w => w.Steps).HasForeignKey(s => s.WorkflowDefinitionId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.WorkflowDefinitionId);
        builder.HasIndex(s => new { s.WorkflowDefinitionId, s.Order });
    }
}

public class WorkflowTransitionConfiguration : IEntityTypeConfiguration<WorkflowTransition>
{
    public void Configure(EntityTypeBuilder<WorkflowTransition> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(t => t.ActionName).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.RequiredPermission).HasMaxLength(200);
        builder.Property(t => t.RowVersion).IsRowVersion();

        builder.HasOne(t => t.WorkflowDefinition).WithMany().HasForeignKey(t => t.WorkflowDefinitionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.FromStep).WithMany(s => s.FromTransitions).HasForeignKey(t => t.FromStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.ToStep).WithMany(s => s.ToTransitions).HasForeignKey(t => t.ToStepId).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new { t.FromStepId, t.ToStepId }).IsUnique();
        builder.HasIndex(t => t.WorkflowDefinitionId);
    }
}
