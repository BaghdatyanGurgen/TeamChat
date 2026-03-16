using TeamChat.API.Hubs;
using TeamChat.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TeamChat.Application.DTOs.Message;
using Microsoft.AspNetCore.Authorization;
using TeamChat.Application.Abstraction.Services;

namespace TeamChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService) : BaseController
{
    private readonly IMessageService _messageService = messageService;

    [Authorize]
    [HttpPost("send")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SendMessage([FromForm] Guid chatId, [FromForm] string? content, IFormFile? attachment)
    {
        var request = new CreateMessageRequest(chatId, content, attachment);
        var result = await _messageService.CreateMessageAsync(CurrentUserId, request);

        if (result.IsSuccess && result.Data is not null)
        {
            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            await hub.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", result.Data);
        }

        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetChatMessages([FromRoute] Guid chatId)
    {
        var result = await _messageService.GetChatMessagesAsync(CurrentUserId, chatId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPatch("{messageId:guid}")]
    public async Task<IActionResult> EditMessage([FromRoute] Guid messageId, [FromBody] EditMessageRequest request)
    {
        var result = await _messageService.EditMessageAsync(CurrentUserId, messageId, request.Content);

        if (result.IsSuccess && result.Data is not null)
        {
            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            await hub.Clients.Group(result.Data.ChatId.ToString()).SendAsync("MessageEdited", result.Data);
        }

        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("{messageId:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
    {
        var result = await _messageService.DeleteMessageAsync(CurrentUserId, messageId);

        if (result.IsSuccess && result.Data is not null)
        {
            var hub = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            await hub.Clients.Group(result.Data.ChatId.ToString())
                .SendAsync("MessageDeleted", new { messageId = result.Data.Id, chatId = result.Data.ChatId });
        }

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{chatId:guid}/read")]
    public async Task<IActionResult> MarkAllAsRead([FromRoute] Guid chatId)
    {
        var result = await _messageService.MarkAllAsReadAsync(chatId, CurrentUserId);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpGet("unread/{companyId:int}")]
    public async Task<IActionResult> GetUnreadCounts([FromRoute] int companyId)
    {
        var result = await _messageService.GetUnreadCountsAsync(CurrentUserId, companyId);
        return result.ToActionResult();
    }
}