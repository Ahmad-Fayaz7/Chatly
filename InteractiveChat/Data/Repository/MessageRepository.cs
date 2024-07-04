using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class MessageRepository(ApplicationDbContext dbContext)
    : Repository<InteractiveChat.Models.Message>(dbContext), IMessageRepository
{
    public async Task<IEnumerable<Message>>? GetPendingMessagesForUserAsync(ApplicationUser? user)
    {
        return await dbSet.Where(m => m.Recipient.Id == user!.Id && m.Status == "pending")
            .Include(m => m.Sender)
            .ToListAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        dbSet.Update(message);
        await dbContext.SaveChangesAsync();
    }
}

