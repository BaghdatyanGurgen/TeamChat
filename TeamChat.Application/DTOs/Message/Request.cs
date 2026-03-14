using Microsoft.AspNetCore.Http;

namespace TeamChat.Application.DTOs.Message;

public record CreateMessageRequest(Guid ChatId, string Content, IFormFile? Attachment = null);