using Asp.Versioning;
using BMedia.Application.Features.MediaAssets.Commands.DeleteMediaAsset;
using BMedia.Application.Features.MediaAssets.Commands.UploadAsset;
using BMedia.Application.Features.MediaAssets.Queries.GetAssets;
using BMedia.Application.Features.MediaAssets.Queries.GetSignedUrl;
using BMedia.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BMedia.API.Controllers.v1;

/// <summary>Media asset management — upload, process, retrieve.</summary>
[ApiVersion("1.0")]
[Authorize]
public class MediaAssetsController : BaseApiController
{
    /// <summary>List media assets with optional filters.</summary>
    [HttpGet]
    [EnableRateLimiting("api")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? contentId,
        [FromQuery] MediaType? mediaType,
        [FromQuery] MediaAssetStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
        => ToActionResult(await Sender.Send(new GetAssetsQuery(contentId, mediaType, status, page, pageSize), cancellationToken));

    /// <summary>Upload a media asset. Supports multipart/form-data for large files.</summary>
    [HttpPost("upload")]
    [EnableRateLimiting("upload")]
    [RequestSizeLimit(5L * 1024 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024)]
    [ProducesResponseType(typeof(UploadAssetResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] Guid? contentId,
        [FromForm] MediaType mediaType,
        [FromForm] string language = "en",
        [FromForm] string? title = null,
        [FromForm] string? description = null,
        [FromForm] string? altText = null,
        [FromForm] bool isPrimary = false,
        [FromForm] int sortOrder = 0,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided" });

        await using var stream = file.OpenReadStream();
        var command = new UploadAssetCommand(stream, file.FileName, file.ContentType, file.Length,
            contentId, mediaType, language, title, description, altText, isPrimary, sortOrder);

        return ToActionResult(await Sender.Send(command, cancellationToken));
    }

    /// <summary>Generate a time-limited signed URL for streaming a private asset.</summary>
    [HttpGet("{id:guid}/url")]
    [EnableRateLimiting("api")]
    [ProducesResponseType(typeof(SignedUrlDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSignedUrl(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new GetSignedUrlQuery(id), cancellationToken));

    /// <summary>Delete (soft-delete) a media asset.</summary>
    [HttpDelete("{id:guid}")]
    [EnableRateLimiting("api")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await Sender.Send(new DeleteMediaAssetCommand(id), cancellationToken));
}
