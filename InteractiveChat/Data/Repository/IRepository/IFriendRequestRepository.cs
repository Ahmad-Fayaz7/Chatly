using InteractiveChat.DTOs;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IFriendRequestRepository : IRepository<FriendRequest>
{
    FriendRequest? GetBySenderAndReceiverIds(string senderId, string receiverId);
    IEnumerable<FriendRequestDTO> GetByReceiverId(string id);
}