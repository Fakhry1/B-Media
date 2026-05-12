using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using MediatR;

namespace BMedia.Application.Features.Tags.Commands;

public record CreateTagCommand(string Name, string? Description) : IRequest<Result<Guid>>;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<Guid>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public CreateTagCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var slug = request.Name.ToLowerInvariant().Replace(" ", "-");
        var tag = new Tag { Name = request.Name, Slug = slug, Description = request.Description };
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Created(tag.Id);
    }
}
