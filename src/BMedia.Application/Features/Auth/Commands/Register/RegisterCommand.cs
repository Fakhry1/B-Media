using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string PreferredLanguage = "en"
) : IRequest<Result<RegisterResult>>;

public record RegisterResult(Guid UserId, string Email, string Username);
