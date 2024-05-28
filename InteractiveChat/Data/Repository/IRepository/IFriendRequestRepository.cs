using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    FriendRequest? FindBySenderAndReceiver(string senderId, string receiverId);
}