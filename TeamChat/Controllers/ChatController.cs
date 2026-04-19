using TeamChat.API.Hubs;
using TeamChat.API.Extensions;
using TeamChat.Domain.Enums;
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
            var chatResponse = new CompanyChatResponse(
                result.Data.Id, result.Data.Name, result.Data.CreatedAt, ChatScope.Company);

            foreach (var participantId in result.Data.ParticipantIds)
            {
                await hub.Clients.User(participantId.ToString())
                    .SendAsync("ChatCreated", chatResponse);
            }
        }

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("private")]
    public async Task<IActionResult> CreatePrivateChat(
        [FromRoute] int companyId, [FromBody] CreatePrivateChatRequest request)
    {
        var result = await _chatService.CreatePrivateChatAsync(
            CurrentUserId, request.TargetUserId, companyId);

        if (result.IsSuccess && result.Data is not null)
        {
            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            var chatResponse = new CompanyChatResponse(
                result.Data.Id, result.Data.Name, result.Data.CreatedAt, ChatScope.Private);

            foreach (var participantId in result.Data.ParticipantIds)
            {
                await hub.Clients.User(participantId.ToString())
                    .SendAsync("ChatCreated", chatResponse);
            }
        }

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{chatId:guid}/positions/attach")]
    public async Task<IActionResult> AttachPosition(
        [FromRoute] int companyId,
        [FromRoute] Guid chatId,
        [FromBody] AttachPositionToChatRequest request)
    {
        PositionPermissions? permOverride = request.PermissionOverride.HasValue
            ? (PositionPermissions)request.PermissionOverride.Value
            : null;

        var result = await _chatService.AttachPositionToChatAsync(
            CurrentUserId, chatId, request.PositionId, permOverride);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{chatId:guid}/positions/detach")]
    public async Task<IActionResult> DetachPosition(
        [FromRoute] int companyId,
        [FromRoute] Guid chatId,
        [FromBody] DetachPositionFromChatRequest request)
    {
        var result = await _chatService.DetachPositionFromChatAsync(
            CurrentUserId, chatId, request.PositionId);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyChats([FromRoute] int companyId)
    {
        var result = await _chatService.GetUserCompanyChatsAsync(CurrentUserId, companyId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("{chatId:guid}")]
    public async Task<IActionResult> DeleteChat(
        [FromRoute] int companyId, [FromRoute] Guid chatId)
    {
        var result = await _chatService.DeleteChatAsync(chatId, CurrentUserId);
        return result.ToActionResult();
    }
}