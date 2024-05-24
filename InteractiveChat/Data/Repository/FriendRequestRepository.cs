using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository
{
    public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
    {
        private ApplicationDbContext _dbContext;
        public FriendRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;

        }

    }
}
