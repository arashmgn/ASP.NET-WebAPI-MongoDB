using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.DTOs;
using AuthService.Application.Mappers;
using AuthService.Domain.Repositories;

namespace AuthService.Application.Services;

public class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthenticationService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        if (await userRepository.GetByEmailAsync(dto.Email, cancellationToken) is not null)
            throw new ConflictException("Email is already taken");

        if (await userRepository.GetByUserNameAsync(dto.UserName, cancellationToken) is not null)
            throw new ConflictException("User Name is already taken");

        var user = dto.ToUserModel(passwordHasher.Hash(dto.Password));
        user.Roles.Add("User");

        await userRepository.CreateAsync(user, cancellationToken);

        return new AuthResponseDto { User = user.ToUserDto(), Token = tokenService.GenerateToken(user) };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email, cancellationToken);

        if (user is null || !passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        return new AuthResponseDto { User = user.ToUserDto(), Token = tokenService.GenerateToken(user) };
    }
}
