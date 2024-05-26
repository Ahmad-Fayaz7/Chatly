using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    void Save();
}