using AuthService.Application.DTOs;

namespace AuthService.Application.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateAsync(string id, UpdateUserDto dto, CancellationToken cancellationToken = default);
    Task UpdatePasswordAsync(string id, UpdatePasswordDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
