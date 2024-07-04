using Newtonsoft.Json;

namespace InteractiveChat.DTOs;

public class ConversationDto
{
    public long ConversationId { get; set; }
    [JsonIgnore]
    public virtual List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    public virtual List<ApplicationUserDTO> Participants { get; set; } = new List<ApplicationUserDTO>();
}