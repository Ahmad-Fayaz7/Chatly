namespace InteractiveChat.DTOs;

public class MessageDto
{
    public long MessageId { get; set; }
    public string SenderUser { get; set; }
    public string RecipientUser { get; set; }
    public string Content { get; set; }
    public string FormattedTimestamp { get; set; }
}