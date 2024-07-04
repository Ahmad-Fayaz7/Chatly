using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IMessageRepository : IRepository<InteractiveChat.Models.Message>
{
    Task<IEnumerable<Message>>? GetPendingMessagesForUserAsync(ApplicationUser? user);
    Task UpdateAsync(Message message);
}