using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.DTOs.User;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Domain.Models.Exceptions.User;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Creates a draft user
    /// </summary>
    [HttpPost("create-draft")]
    public async Task<ResponseModel<RegisterEmailResponse>> CreateDraftUser([FromBody] CreateDraftUserRequest request)
        => await _userService.CreateDraftUserAsync(request);

    /// <summary>
    /// Confirms user's email address
    /// </summary>
    [HttpPost("verify-email")]
    public async Task<ResponseModel<VerifyEmailResponse>> VerifyEmail([FromBody] VerivyEmailRequest request)
         => await _userService.VerifyEmailAsync(request);

    /// <summary>
    /// Completes user registration by setting a password
    /// </summary>
    [HttpPost("set-password")]
    public async Task<ResponseModel<SetPasswordResponse>> SetPassword([FromBody] SetPasswordRequest request)
        => await _userService.SetPasswordAsync(request);

    /// <summary>
    /// Sets user profile information
    /// </summary>
    [Authorize]
    [HttpGet("set-user-profile")]
    public async Task<ResponseModel<UserProfileResponse>> SetUserProfile([FromQuery] SetUserProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuidId))
            throw new UserNotFoundException();

        return await _userService.SetUserProfileAsync(userGuidId, request);
    }
}