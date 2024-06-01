using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository;

public interface IFriendshipRepository : IRepository<Friendship>
{
    IEnumerable<Friendship> GetAll();
}