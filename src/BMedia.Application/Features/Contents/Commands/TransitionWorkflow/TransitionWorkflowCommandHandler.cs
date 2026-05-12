using BMedia.Application.Common.Interfaces;
using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Commands.TransitionWorkflow;

public class TransitionWorkflowCommandHandler : IRequestHandler<TransitionWorkflowCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;
    private readonly INotificationService _notificationService;
    private readonly ISearchService _searchService;

    public TransitionWorkflowCommandHandler(ApplicationDbContext db, ICurrentUserService currentUser,
        INotificationService notificationService, ISearchService searchService)
    {
        _db = db;
        _currentUser = currentUser;
        _notificationService = notificationService;
        _searchService = searchService;
    }

    public async Task<Result<bool>> Handle(TransitionWorkflowCommand request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents
            .Include(c => c.CurrentWorkflowStep)
            .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

        if (content is null) return Result<bool>.NotFound("Content not found");

        var transition = await _db.WorkflowTransitions
            .Include(t => t.FromStep)
            .Include(t => t.ToStep)
            .FirstOrDefaultAsync(t => t.Id == request.TransitionId && t.IsActive, cancellationToken);

        if (transition is null) return Result<bool>.NotFound("Workflow transition not found");

        if (content.CurrentWorkflowStepId != transition.FromStepId)
            return Result<bool>.Failure("Content is not at the required workflow step for this transition");

        if (transition.RequiresComment && string.IsNullOrWhiteSpace(request.Comment))
            return Result<bool>.Failure("A comment is required for this transition");

        if (!string.IsNullOrEmpty(transition.RequiredPermission) && !_currentUser.HasPermission(transition.RequiredPermission))
            return Result<bool>.Forbidden($"Permission '{transition.RequiredPermission}' is required");

        var previousStatus = content.Status;
        content.CurrentWorkflowStepId = transition.ToStepId;
        content.Status = transition.ToStep.MapsToStatus;

        if (transition.ToStep.IsFinal && content.Status == Domain.Enums.ContentStatus.Published)
        {
            content.PublishedAt = DateTime.UtcNow;
            content.PublishedBy = _currentUser.UserId;
        }

        var history = new ContentWorkflowHistory
        {
            ContentId = content.Id,
            FromStepId = transition.FromStepId,
            ToStepId = transition.ToStepId,
            TransitionedById = _currentUser.UserId!.Value,
            FromStatus = previousStatus,
            ToStatus = content.Status,
            Comment = request.Comment,
            ActionName = transition.ActionName,
            TransitionedAt = DateTime.UtcNow
        };

        _db.ContentWorkflowHistories.Add(history);
        await _db.SaveChangesAsync(cancellationToken);

        // Async side effects
        if (content.Status == Domain.Enums.ContentStatus.Published)
            await _searchService.IndexContentAsync(content.Id, cancellationToken);

        return Result<bool>.Success(true);
    }
}
