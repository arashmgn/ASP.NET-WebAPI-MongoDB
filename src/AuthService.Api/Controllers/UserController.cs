using System.Security.Claims;
using AuthService.Application.Common.Exceptions;
using AuthService.Application.DTOs;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get(CancellationToken cancellationToken)
    {
        return Ok(await userService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(string id, CancellationToken cancellationToken)
    {
        return Ok(await userService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(string id, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        EnsureSelfOrAdmin(id);
        return Ok(await userService.UpdateAsync(id, dto, cancellationToken));
    }

    [HttpPut("{id}/password")]
    public async Task<IActionResult> UpdatePassword(string id, UpdatePasswordDto dto, CancellationToken cancellationToken)
    {
        EnsureSelfOrAdmin(id);
        await userService.UpdatePasswordAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        EnsureSelfOrAdmin(id);
        await userService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    private void EnsureSelfOrAdmin(string id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (currentUserId != id && !User.IsInRole("Admin"))
            throw new ForbiddenException("You can only modify your own account");
    }
}
