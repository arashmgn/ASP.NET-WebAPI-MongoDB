using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
