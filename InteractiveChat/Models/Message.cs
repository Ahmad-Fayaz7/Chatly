namespace InteractiveChat.Models;

public class Message
{
    public long ConversationId { get; set; }
    public long MessageId { get; set; }
    public string SenderId { get; set; }
    public string RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }
    public string MessageType { get; set; } // "text", "image", "video", etc.
    public string MediaUrl { get; set; } // URL of the media file

    // Additional metadata properties (optional)
    public long? FileSize { get; set; }
    public TimeSpan? Duration { get; set; }
    public string ThumbnailUrl { get; set; }

    // Navigation properties
    public virtual Conversation Conversation { get; set; }
    public virtual ApplicationUser Sender { get; set; }
    public virtual ApplicationUser Recipient { get; set; }
    
}