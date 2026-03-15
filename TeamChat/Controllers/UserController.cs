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

    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserProfile([FromRoute] Guid userId)
    {
        var result = await _userService.GetUserProfileAsync(userId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.ChangePasswordAsync(CurrentUserId, request.OldPassword, request.NewPassword);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SetAvatar(IFormFile avatar)
    {
        if (avatar == null || avatar.Length == 0)
            return BadRequest(new { Message = "No file provided." });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(avatar.ContentType.ToLower()))
            return BadRequest(new { Message = "Only JPEG, PNG and WebP images are allowed." });

        if (avatar.Length > 5 * 1024 * 1024)
            return BadRequest(new { Message = "File size must not exceed 5 MB." });
        
        var result = await _userService.SetUserAvatarAsync(CurrentUserId, avatar);
        return Ok(result);
    }
}