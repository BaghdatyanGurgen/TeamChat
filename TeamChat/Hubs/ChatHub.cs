using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TeamChat.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        await base.OnConnectedAsync();
    }

    public async Task SendMessageToChat(string chatId, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task JoinCompany(string companyId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"company-{companyId}");
    }

    public async Task LeaveCompany(string companyId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"company-{companyId}");
    }
}