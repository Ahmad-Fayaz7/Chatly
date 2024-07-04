using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IConversationRepository : IRepository<Conversation>
{
    public IEnumerable<Conversation> GetAll();
    public Conversation? GetByRecipients(ApplicationUser? userA, ApplicationUser? userB);
    public IEnumerable<Conversation>? GetAllConversationsOfUser(ApplicationUser? user);

}