using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Categories.Commands;

public record CreateCategoryCommand(string Name, string? Description, string? IconUrl, int SortOrder = 0) : IRequest<Result<Guid>>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public CreateCategoryCommandHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = request.Name.ToLowerInvariant().Replace(" ", "-");
        var category = new Domain.Entities.Category
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder
        };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Created(category.Id);
    }
}
