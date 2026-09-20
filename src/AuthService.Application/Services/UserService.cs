using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.DTOs;
using AuthService.Application.Mappers;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Select(u => u.ToUserDto());
    }

    public async Task<UserDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await FindAsync(id, cancellationToken);
        return user.ToUserDto();
    }

    public async Task<UserDto> UpdateAsync(string id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await FindAsync(id, cancellationToken);

        var byEmail = await userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (byEmail is not null && byEmail.Id != id)
            throw new ConflictException("Email is already taken");

        var byUserName = await userRepository.GetByUserNameAsync(dto.UserName, cancellationToken);
        if (byUserName is not null && byUserName.Id != id)
            throw new ConflictException("User Name is already taken");

        user.UserName = dto.UserName;
        user.Email = dto.Email;

        await userRepository.UpdateAsync(id, user, cancellationToken);
        return user.ToUserDto();
    }

    public async Task UpdatePasswordAsync(string id, UpdatePasswordDto dto, CancellationToken cancellationToken = default)
    {
        var user = await FindAsync(id, cancellationToken);
        user.PasswordHash = passwordHasher.Hash(dto.Password);
        await userRepository.UpdateAsync(id, user, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await FindAsync(id, cancellationToken);
        await userRepository.DeleteAsync(id, cancellationToken);
    }

    private async Task<User> FindAsync(string id, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException("User not found");
    }
}
