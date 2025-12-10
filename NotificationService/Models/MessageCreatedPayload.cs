namespace NotificationService.Models;

public class MessageCreatedPayload
{
    public Guid ChatId { get; set; }
    public Guid MessageId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = "";
    public DateTime SentAt { get; set; }
}