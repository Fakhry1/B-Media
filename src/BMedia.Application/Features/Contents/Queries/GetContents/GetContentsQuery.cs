using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;

namespace BMedia.Application.Features.Contents.Queries.GetContents;

public record GetContentsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    ContentStatus? Status = null,
    Guid? CategoryId = null,
    Guid? SubcategoryId = null,
    string? Language = null,
    bool? IsFeatured = null,
    string? SortBy = null,
    bool SortDescending = true,
    MediaType? MediaType = null
) : IRequest<Result<PaginatedResult<ContentListDto>>>;

public record ContentListDto(
    Guid Id,
    string Title,
    string Slug,
    string? Summary,
    string Language,
    ContentStatus Status,
    bool IsFeatured,
    DateTime CreatedAt,
    DateTime? PublishedAt,
    string? CategoryName,
    string? ThumbnailUrl,
    IEnumerable<string> Tags
);
