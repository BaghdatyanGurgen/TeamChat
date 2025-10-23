namespace TeamChat.Domain.Entities;

public class MessageAttachment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MessageId { get; set; }
    public string FileUrl { get; set; } = string.Empty;

    public Message Message { get; set; } = null!;
}
