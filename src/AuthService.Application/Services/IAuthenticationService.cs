using AuthService.Application.DTOs;

namespace AuthService.Application.Services;

public interface IAuthenticationService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
}
