using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Contents.Commands.CreateContent;

public record CreateContentCommand(
    string Title,
    string? Summary,
    string Language,
    Guid? CategoryId,
    Guid? SubcategoryId,
    IEnumerable<Guid>? TagIds,
    DateTime? ScheduledPublishAt
) : IRequest<Result<CreateContentResult>>;

public record CreateContentResult(Guid ContentId, string Slug);
