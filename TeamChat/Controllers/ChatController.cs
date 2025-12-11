using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Domain.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.Abstraction.Services;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController(IChatService chatService) : ControllerBase
{
    private readonly IChatService _chatService = chatService;

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            throw new UserNotFoundException();

        var result = await _chatService.CreateChatAsync(userGuid, request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}