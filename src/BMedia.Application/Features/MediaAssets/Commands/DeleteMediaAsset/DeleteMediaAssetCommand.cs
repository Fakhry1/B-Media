using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.MediaAssets.Commands.DeleteMediaAsset;

public record DeleteMediaAssetCommand(Guid AssetId) : IRequest<Result<bool>>;

public class DeleteMediaAssetCommandHandler : IRequestHandler<DeleteMediaAssetCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;

    public DeleteMediaAssetCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(DeleteMediaAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _db.MediaAssets
            .FirstOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);

        if (asset is null)
            return Result<bool>.NotFound("Media asset not found");

        asset.IsDeleted = true;
        asset.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
