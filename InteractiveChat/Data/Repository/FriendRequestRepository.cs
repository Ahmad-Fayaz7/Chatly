using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository;

public class FriendRequestRepository(ApplicationDbContext dbContext)
    : Repository<FriendRequest>(dbContext), IFriendRequestRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public FriendRequest? FindBySenderAndReceiver(string senderId, string receiverId)
    {
        return _dbContext.FriendRequests
            .FirstOrDefault(request => request.SenderId == senderId && request.ReceiverId == receiverId );
    }
}