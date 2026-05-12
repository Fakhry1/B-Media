using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Contents.Commands.TransitionWorkflow;

public record TransitionWorkflowCommand(
    Guid ContentId,
    Guid TransitionId,
    string? Comment
) : IRequest<Result<bool>>;
