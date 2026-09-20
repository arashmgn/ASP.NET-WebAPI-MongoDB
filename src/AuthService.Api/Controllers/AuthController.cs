using AuthService.Application.DTOs;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        return Ok(await authenticationService.RegisterAsync(dto, cancellationToken));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        return Ok(await authenticationService.LoginAsync(dto, cancellationToken));
    }
}
