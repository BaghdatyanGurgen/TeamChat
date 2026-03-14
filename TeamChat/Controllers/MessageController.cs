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
    public async Task<IActionResult> SendMessage([FromBody] CreateMessageRequest request)
    {
        var result = await _messageService.CreateMessageAsync(CurrentUserId, request);

        if (result.IsSuccess && result.Data is not null)
        {
            var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<ChatHub>>();
            await hubContext.Clients.Group(request.ChatId.ToString())
                .SendAsync("ReceiveMessage", result.Data);
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
}