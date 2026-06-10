using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Contents.Commands.UpdateContent;

public record UpdateContentCommand(
    Guid ContentId,
    string Title,
    string? Summary,
    string Language,
    Guid? CategoryId,
    Guid? SubcategoryId,
    bool IsFeatured,
    bool AllowComments,
    IEnumerable<Guid>? TagIds
) : IRequest<Result<bool>>;
