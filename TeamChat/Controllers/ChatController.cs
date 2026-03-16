using TeamChat.API.Hubs;
using TeamChat.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        if (result.IsSuccess && result.Data is not null)
        {
            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            var chatResponse = new CompanyChatResponse(result.Data.Id, result.Data.Name, result.Data.CreatedAt);

            foreach (var participantId in result.Data.ParticipantIds)
            {
                await hub.Clients.User(participantId.ToString()).SendAsync("ChatCreated", chatResponse);
            }
        }

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