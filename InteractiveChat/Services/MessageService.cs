using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using InteractiveChat.Services.IServices;

namespace InteractiveChat.Services;

public class MessageService(IConversationRepository conversationRepository) : IMessageService
{
   public IEnumerable<ConversationDto>? GetAllConversationsOfUser(ApplicationUser? user)
   {
      var timeZone = TimeZoneInfo.Local;
      if (user == null)
      {
         return null;
      }
      var conversations = conversationRepository.GetAllConversationsOfUser(user);
      
      // Transform entities to DTOs
      var conversationDtos = conversations.Select(c => new ConversationDto()
      {
         ConversationId = c.ConversationId,
         Participants = c.Participants
            .Where(p => p.UserName != user.UserName)
            .Select(p => new ApplicationUserDTO()
            {
               FirstName = p.FirstName,
               LastName = p.LastName,
               ProfilePicUrl = p.ProfilePicUrl,
               UserName = p.UserName
            }).ToList(),
         Messages = c.Messages.Select(m => new MessageDto()
         {
            MessageId = m.MessageId,
            Content = m.Content,
            FormattedTimestamp = TimeZoneInfo.ConvertTimeFromUtc(m.Timestamp, timeZone).ToString("HH:mm") }).ToList()
         
      });
      return conversationDtos;
   }

   public ConversationDto GetConversationByParticipants(ApplicationUser senderUser, ApplicationUser recipientUser)
   {
      var timeZone = TimeZoneInfo.Local;
      var conversation = conversationRepository.GetByRecipients(senderUser, recipientUser);
      var conversationDto = new ConversationDto
      {
         ConversationId = conversation.ConversationId,
         Messages = conversation.Messages.OrderBy(m => m.Timestamp).Select(m => new MessageDto()
         {
            Content = m.Content,
            FormattedTimestamp = TimeZoneInfo.ConvertTimeFromUtc(m.Timestamp, timeZone).ToString("HH:mm"),
            MessageId = m.MessageId,
            SenderUser = m.Sender.UserName,
            RecipientUser = m.Recipient.UserName
         }).ToList(),
         Participants = conversation.Participants
            .Select(p => new ApplicationUserDTO()
            {
               FirstName = p.FirstName,
               LastName = p.LastName,
               ProfilePicUrl = p.ProfilePicUrl,
               UserName = p.UserName
            }).ToList(),
         
      };
      return conversationDto;
   }
}