using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class ConversationRepository(ApplicationDbContext dbContext)
    : Repository<Conversation>(dbContext), IConversationRepository
{
    public IEnumerable<Conversation> GetAll()
    {
        return dbSet.Include(c => c.Participants);
    }

    public Conversation? GetByRecipients(ApplicationUser? userA, ApplicationUser? userB)
    {
        var conversation = dbSet.Include(c => c.Participants)
            .Include(c => c.Messages)
            .FirstOrDefault(c => c.Participants.Count == 2 &&
                                 c.Participants.Any(p => p.Id == userA.Id) &&
                                 c.Participants.Any(p => p.Id == userB.Id));
        return conversation;
    }

    public IEnumerable<Conversation>? GetAllConversationsOfUser(ApplicationUser? user)
    {
        var conversationList = dbSet
            .Where(c => c.Participants.Any(p => p.Id == user.Id))
            .Include(c => c.Participants)
            .Include(c => c.Messages)
            .ToList();
        return conversationList;
    }
}