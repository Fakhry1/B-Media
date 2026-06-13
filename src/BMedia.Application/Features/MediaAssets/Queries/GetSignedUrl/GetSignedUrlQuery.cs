using BMedia.Application.Common.Models;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.MediaAssets.Queries.GetSignedUrl;

public record GetSignedUrlQuery(Guid AssetId) : IRequest<Result<SignedUrlDto>>;

public record SignedUrlDto(string Url, DateTimeOffset ExpiresAt);

public class GetSignedUrlQueryHandler : IRequestHandler<GetSignedUrlQuery, Result<SignedUrlDto>>
{
    private readonly ApplicationDbContext _db;
    private readonly IStorageService _storage;

    public GetSignedUrlQueryHandler(ApplicationDbContext db, IStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<Result<SignedUrlDto>> Handle(GetSignedUrlQuery request, CancellationToken cancellationToken)
    {
        var asset = await _db.MediaAssets
            .FirstOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);

        if (asset is null)
            return Result<SignedUrlDto>.NotFound("Media asset not found");

        var expiry = TimeSpan.FromHours(2);
        var url = await _storage.GenerateSignedUrlAsync(asset.StorageKey, expiry, cancellationToken);

        return Result<SignedUrlDto>.Success(new SignedUrlDto(url, DateTimeOffset.UtcNow.Add(expiry)));
    }
}
