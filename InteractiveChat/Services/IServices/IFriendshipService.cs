using InteractiveChat.Models.ViewModels;

namespace InteractiveChat.Services.IServices
{
    public interface IFriendshipService
    {
        Task<Result> SendFriendRequest(string senderUsername, string receiverUsername);

        List<SearchResultViewModel> SearchFriend(string? loggedInUserId, string searchTerm);
    }
}
