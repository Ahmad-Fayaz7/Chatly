using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class FriendshipRepository(ApplicationDbContext dbContext)
    : Repository<Friendship>(dbContext), IFriendshipRepository
{
    public readonly ApplicationDbContext _dbContext = dbContext;


    public IEnumerable<Friendship> GetAll()
    {
        return dbSet
            .Include(fr => fr.User)
            .Include(fr => fr.Friend);
    }
}