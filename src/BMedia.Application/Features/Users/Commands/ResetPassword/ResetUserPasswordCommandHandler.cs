using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using BMedia.Infrastructure.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Commands.ResetPassword;

public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public ResetUserPasswordCommandHandler(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<bool>> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user is null)
            return Result<bool>.NotFound("User not found");

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
