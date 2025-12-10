using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs.Message;
using TeamChat.Application.Services;
using TeamChat.Domain.Models.Exceptions.User;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    private readonly IMessageService _messageService = messageService;

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] CreateMessageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var guidUserId))
            throw new UserNotFoundException();

        var result = await _messageService.CreateMessageAsync(guidUserId, request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetChatMessages([FromRoute] Guid chatId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var guidUserId))
            throw new UserNotFoundException();

        var result = await _messageService.GetChatMessagesAsync(guidUserId, chatId);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
