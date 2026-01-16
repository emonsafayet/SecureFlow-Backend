using MediatR;
using SecureFlow.Application.Auth.DTOs;

namespace SecureFlow.Application.Auth.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponseDto>;
