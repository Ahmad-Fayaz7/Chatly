using InteractiveChat.DTOs;
using InteractiveChat.Models;

namespace InteractiveChat.Services.IServices;

public interface IMessageService
{
    public IEnumerable<ConversationDto>? GetAllConversationsOfUser(ApplicationUser? user);
    public ConversationDto GetConversationByParticipants(ApplicationUser senderUser, ApplicationUser recipientUser);
}