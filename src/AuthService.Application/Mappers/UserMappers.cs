using AuthService.Application.DTOs;
using AuthService.Domain.Entities;

namespace AuthService.Application.Mappers;

public static class UserMappers
{
    public static User ToUserModel(this RegisterDto dto, string passwordHash)
    {
        return new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = passwordHash
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }
}
