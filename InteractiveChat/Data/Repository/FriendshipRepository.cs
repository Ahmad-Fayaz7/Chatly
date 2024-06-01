using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
{
    public readonly ApplicationDbContext _dbContext;
    public FriendshipRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public IEnumerable<Friendship> GetAll()
    {
        return dbSet
            .Include(fr => fr.User)
            .Include(fr => fr.Friend);
    }
}