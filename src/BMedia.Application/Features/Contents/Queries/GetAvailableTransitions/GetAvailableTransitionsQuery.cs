using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Queries.GetAvailableTransitions;

public record GetAvailableTransitionsQuery(Guid ContentId) : IRequest<Result<IEnumerable<WorkflowTransitionDto>>>;

public record WorkflowTransitionDto(
    Guid TransitionId,
    string ActionName,
    string? Description,
    string TargetStepName,
    bool RequiresComment
);

public class GetAvailableTransitionsQueryHandler
    : IRequestHandler<GetAvailableTransitionsQuery, Result<IEnumerable<WorkflowTransitionDto>>>
{
    private readonly ApplicationDbContext _db;

    public GetAvailableTransitionsQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<IEnumerable<WorkflowTransitionDto>>> Handle(
        GetAvailableTransitionsQuery request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents
            .AsNoTracking()
            .Select(c => new { c.Id, c.CurrentWorkflowStepId })
            .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

        if (content is null)
            return Result<IEnumerable<WorkflowTransitionDto>>.NotFound("Content not found");

        var transitions = await _db.WorkflowTransitions
            .AsNoTracking()
            .Include(t => t.ToStep)
            .Where(t => t.FromStepId == content.CurrentWorkflowStepId && t.IsActive)
            .OrderBy(t => t.ToStep.Order)
            .Select(t => new WorkflowTransitionDto(
                t.Id,
                t.ActionName,
                t.Description,
                t.ToStep.Name,
                t.RequiresComment))
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<WorkflowTransitionDto>>.Success(transitions);
    }
}
