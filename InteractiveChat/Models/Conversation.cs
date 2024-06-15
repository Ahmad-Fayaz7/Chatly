namespace InteractiveChat.Models;

public class Conversation
{
    public long ConversationId { get; set; }
    public DateTime CreatedTimestamp { get; set; }
    public DateTime LastUpdatedTimestamp { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }

    // Navigation property
    public virtual List<Message> Messages { get; set; } = new List<Message>();
    public virtual List<ApplicationUser> Participants { get; set; } = new List<ApplicationUser>();

    // Default constructor
    public Conversation()
    {
        CreatedTimestamp = DateTime.UtcNow;
        LastUpdatedTimestamp = DateTime.UtcNow;
        Status = "active";
    }
    
}