using TeamChat.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Application.DTOs.Chat;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.Abstraction.Services;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/company/{companyId}/chats")]
public class ChatController(IChatService chatService) : BaseController
{
    private readonly IChatService _chatService = chatService;

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromRoute] int companyId, [FromBody] CreateChatRequest request)
    {
        var result = await _chatService.CreateChatAsync(CurrentUserId, request);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyChats([FromRoute] int companyId)
  {
        var result = await _chatService.GetUserCompanyChatsAsync(CurrentUserId, companyId);

        return result.ToActionResult();
    }
}