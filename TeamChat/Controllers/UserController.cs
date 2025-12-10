using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.DTOs.User;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Domain.Models.Exceptions.User;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create-draft")]
    public async Task<IActionResult> CreateDraftUser([FromBody] CreateDraftUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.CreateDraftUserAsync(request);
        return Ok(result);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.VerifyEmailAsync(request);
        return Ok(result);
    }

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.SetPasswordAsync(request);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch("set-user-profile")]
    public async Task<IActionResult> SetUserProfile([FromBody] SetUserProfileRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuidId))
            throw new UserNotFoundException();

        var result = await _userService.SetUserProfileAsync(userGuidId, request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.RefreshTokenAsync(request.Token, request.RefreshToken);
        if (!result.IsSuccess)
            return Unauthorized(result.Message);

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuidId))
            throw new UserNotFoundException();

        await _userService.LogoutAsync(userGuidId);
        return Ok(new { Message = "Logged out successfully" });
    }

}
