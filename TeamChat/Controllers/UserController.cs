using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChat.API.Extensions;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs.User;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

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

        var result = await _userService.SetUserProfileAsync(CurrentUserId, request);
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

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync(CurrentUserId);
        return Ok(new { Message = "Logged out successfully" });
    }
}