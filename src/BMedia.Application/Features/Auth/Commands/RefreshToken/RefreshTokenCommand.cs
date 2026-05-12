using BMedia.Application.Common.Models;
using BMedia.Application.Features.Auth.Commands.Login;
using MediatR;

namespace BMedia.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken, string? IpAddress) : IRequest<Result<LoginResult>>;
