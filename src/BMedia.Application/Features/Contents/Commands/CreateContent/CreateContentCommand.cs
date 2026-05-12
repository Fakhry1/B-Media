using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Contents.Commands.CreateContent;

public record CreateContentCommand(
    string Title,
    string? Summary,
    string? Body,
    string Language,
    Guid? CategoryId,
    Guid? SubcategoryId,
    string? SeoTitle,
    string? SeoDescription,
    string? SeoKeywords,
    IEnumerable<Guid>? TagIds,
    DateTime? ScheduledPublishAt
) : IRequest<Result<CreateContentResult>>;

public record CreateContentResult(Guid ContentId, string Slug);
