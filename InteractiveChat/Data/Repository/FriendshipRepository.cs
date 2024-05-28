using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository;

public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
{
    public readonly ApplicationDbContext _dbContext;
    public FriendshipRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    
}