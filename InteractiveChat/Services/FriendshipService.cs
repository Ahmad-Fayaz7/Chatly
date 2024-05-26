using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    

    public FriendshipService(UserManager<ApplicationUser> userManager,
        IApplicationUserRepository applicationUserRepository, IFriendRequestRepository friendRequestRepository)
    {
        _userManager = userManager;
        _applicationUserRepository = applicationUserRepository;
        _friendRequestRepository = friendRequestRepository;
    }
    

    public List<SearchResultViewModel> SearchFriend(ApplicationUser? loggedInUser, string searchTerm)
    {
        var searchResultViewModels = new List<SearchResultViewModel>();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();
            var usersWithRelationships = _userManager.Users
                .Include(u => u.SentFriendRequests) // Include sent friend requests
                .Include(u => u.ReceivedFriendRequests) // Include received friend requests
                .Include(u => u.Friendships) // Include friendships
                .Where(u => (u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm)) && u.Id != loggedInUser.Id)
                .ToList();

            searchResultViewModels = CreateSearchResultViewModel(usersWithRelationships, loggedInUser.Id);
        }

        return searchResultViewModels;
    }

    public Result CancelFriendRequest(ApplicationUser loggedInUser, string username)
    {
        var receiver = _applicationUserRepository.GetByUsername(username);
        var friendRequestToDelete = new FriendRequest { SenderId = loggedInUser.Id, ReceiverId = receiver.Id };
        _friendRequestRepository.Delete(friendRequestToDelete);
        return Result.Success();
    }

    public Result RejectFriendRequest(ApplicationUser loggedInUser, string username)
    {
        var sender = _applicationUserRepository.GetByUsername(username);
        var friendRequestToDelete = new FriendRequest { SenderId = sender.Id ,ReceiverId = loggedInUser.Id };
        _friendRequestRepository.Delete(friendRequestToDelete);
        return Result.Success();
    }

    public async Task<Result> SendFriendRequest(string senderUsername, string receiverUsername)
    {
        // Implement the logic to send a friend request
        var sender = _applicationUserRepository.GetByUsername(senderUsername);
        var receiver = _applicationUserRepository.GetByUsername(receiverUsername);

        if (sender == null || receiver == null) return Result.Error("Sender or receiver not found.");

        // Send the friend request...
        var friendRequest = new FriendRequest { ReceiverId = receiver.Id, SenderId = sender.Id };
        _friendRequestRepository.Add(friendRequest);
        _friendRequestRepository.Save();

        return Result.Success();
    }


    private List<SearchResultViewModel> CreateSearchResultViewModel(List<ApplicationUser> usersWithRelationships,
        string loggedInUserId)
    {
        var searchResultViewModels = new List<SearchResultViewModel>();
        // Process the search results
        foreach (var user in usersWithRelationships)
        {
            RelationshipStatus relationshipStatus;
            // Filter and match friend requests and friendships for the searching user and the searched users
            var sentFriendRequestToUser = user.SentFriendRequests.FirstOrDefault(fr => fr.ReceiverId == loggedInUserId);
            var receivedFriendRequestFromUser =
                user.ReceivedFriendRequests.FirstOrDefault(fr => fr.SenderId == loggedInUserId);
            var friendshipWithUser =
                user.Friendships.FirstOrDefault(f => f.UserId == loggedInUserId || f.FriendId == loggedInUserId);

            // Use the retrieved data to determine the relationship status between the searching user and the searched user
            if (sentFriendRequestToUser != null)
                relationshipStatus = RelationshipStatus.ReceivedRequest;
            else if (receivedFriendRequestFromUser != null)
                relationshipStatus = RelationshipStatus.PendingRequest;
            else if (friendshipWithUser != null)
                relationshipStatus = RelationshipStatus.Accepted;
            else
                relationshipStatus = RelationshipStatus.None;
            searchResultViewModels.Add(new SearchResultViewModel
                { User = user, RelationshipStatus = relationshipStatus });
        }

        return searchResultViewModels;
    }
}