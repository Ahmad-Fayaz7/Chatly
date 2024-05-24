using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendshipService(UserManager<ApplicationUser> userManager, IApplicationUserRepository applicationUserRepository, IFriendRequestRepository friendRequestRepository)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _friendRequestRepository = friendRequestRepository;
        }

        public FriendshipService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<SearchResultViewModel> SearchFriend(string? loggedInUserId, string searchTerm)
        {
            List<SearchResultViewModel> searchResultViewModels = new List<SearchResultViewModel>();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                var usersWithRelationships = _userManager.Users
                    .Include(u => u.SentFriendRequests) // Include sent friend requests
                    .Include(u => u.ReceivedFriendRequests) // Include received friend requests
                    .Include(u => u.Friendships) // Include friendships
                    .Where(u => u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm))
                    .ToList();

                searchResultViewModels = CreateSearchResultViewModel(usersWithRelationships, loggedInUserId);
            }
            return searchResultViewModels;

        }

        public async Task<Result> SendFriendRequest(string senderUsername, string receiverUsername)
        {
            // Implement the logic to send a friend request
            var sender = _applicationUserRepository.GetByUsername(senderUsername);
            var receiver = _applicationUserRepository.GetByUsername(receiverUsername);

            if (sender == null || receiver == null)
            {
                return Result.Error("Sender or receiver not found.");
            }

            // Send the friend request...
            var friendRequest = new FriendRequest { ReceiverId = receiver.Id, SenderId = sender.Id };
            _friendRequestRepository.Add(friendRequest);

            return Result.Success();
        }


        private List<SearchResultViewModel> CreateSearchResultViewModel(List<ApplicationUser> usersWithRelationships, string loggedInUserId)
        {

            List<SearchResultViewModel> searchResultViewModels = new List<SearchResultViewModel>();
            // Process the search results
            foreach (var user in usersWithRelationships)
            {
                RelationshipStatus relationshipStatus;
                // Filter and match friend requests and friendships for the searching user and the searched users
                var sentFriendRequestToUser = user.SentFriendRequests.FirstOrDefault(fr => fr.ReceiverId == loggedInUserId);
                var receivedFriendRequestFromUser = user.ReceivedFriendRequests.FirstOrDefault(fr => fr.SenderId == loggedInUserId);
                var friendshipWithUser = user.Friendships.FirstOrDefault(f => f.UserId == loggedInUserId || f.FriendId == loggedInUserId);

                // Use the retrieved data to determine the relationship status between the searching user and the searched user
                if (sentFriendRequestToUser != null)
                {
                    relationshipStatus = RelationshipStatus.ReceivedRequest;
                }
                else if (receivedFriendRequestFromUser != null)
                {
                    relationshipStatus = RelationshipStatus.PendingRequest;
                }
                else if (friendshipWithUser != null)
                {
                    relationshipStatus = RelationshipStatus.Accepted;
                }
                else
                {
                    relationshipStatus = RelationshipStatus.None;
                }
                searchResultViewModels.Add(new SearchResultViewModel { User = user, RelationshipStatus = relationshipStatus });
            }
            return searchResultViewModels;
        }
    }
}
