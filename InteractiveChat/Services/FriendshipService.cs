using InteractiveChat.Data;
using InteractiveChat.Data.Repository;
using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace InteractiveChat.Services;

public class FriendshipService(
    UserManager<ApplicationUser> userManager,
    IApplicationUserRepository applicationUserRepository,
    IFriendRequestRepository friendRequestRepository,
    IFriendshipRepository friendshipRepository,
    ApplicationDbContext dbContext
    )
    : IFriendshipService
{
    public List<SearchResultViewModel> SearchFriend(ApplicationUser? loggedInUser, string searchTerm)
    {
        var searchResultViewModels = new List<SearchResultViewModel>();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();
            var usersWithRelationships = userManager.Users
                .Include(u => u.SentFriendRequests) // Include sent friend requests
                .Include(u => u.ReceivedFriendRequests) // Include received friend requests
                .Include(u => u.Friendships) // Include friendships
                .Where(u => (u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm)) &&
                            u.Id != loggedInUser.Id) // Exclude the searching user(Logged in user) 
                .ToList();

            searchResultViewModels = CreateSearchResultViewModel(usersWithRelationships, loggedInUser.Id);
        }

        return searchResultViewModels;
    }

    public async Task<Result> SendFriendRequest(string senderUsername, string receiverUsername)
    {
        // Implement the logic to send a friend request
        var sender = applicationUserRepository.GetByUsername(senderUsername);
        var receiver = applicationUserRepository.GetByUsername(receiverUsername);

        if (sender == null || receiver == null) return Result.Error("Sender or receiver not found.");

        // Send the friend request...
        var friendRequest = new FriendRequest { ReceiverId = receiver.Id, SenderId = sender.Id };
        friendRequestRepository.Add(friendRequest);

        return Result.Success();
    }

    public Result CancelFriendRequest(ApplicationUser loggedInUser, string username)
    {
        var receiver = applicationUserRepository.GetByUsername(username);
        var friendRequestToDelete = new FriendRequest { SenderId = loggedInUser.Id, ReceiverId = receiver.Id };
        friendRequestRepository.Delete(friendRequestToDelete);
        return Result.Success();
    }

    public Result AcceptFriendRequest(ApplicationUser? loggedInUser, string username)
    {
        if (loggedInUser == null)
        {
            return Result.Error("Logged in user cannot be null.");
        }

        if (string.IsNullOrEmpty(username))
        {
            return Result.Error("Username cannot be null or empty.");
        }

        try
        {
            var sender = applicationUserRepository.GetByUsername(username);
            if (sender == null)
            {
                return Result.Error("The specified user does not exists.");
            }

            var friendRequestToDelete = friendRequestRepository.FindBySenderAndReceiver(sender.Id, loggedInUser.Id);
            if (friendRequestToDelete == null)
            {
                return Result.Error("Friend request does not exist.");
            }

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Friendship friendship = new Friendship() { UserId = loggedInUser.Id, FriendId = sender.Id, FriendshipDate = DateTime.Now};        
                    friendshipRepository.Add(friendship);
                    friendRequestRepository.Delete(friendRequestToDelete);
                    dbContext.SaveChanges();
                    transaction.Commit();
                    return Result.Success();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Result.Error("An error occurred while processing your request. Please try again later.");
                }
                
            }
        }
        catch (Exception e)
        {
            return Result.Error("An error occurred while processing your request. Please try again later.");
        }
    }

    public Result RejectFriendRequest(ApplicationUser loggedInUser, string username)
    {
        var sender = applicationUserRepository.GetByUsername(username);
        var friendRequestToDelete = new FriendRequest { SenderId = sender.Id, ReceiverId = loggedInUser.Id };
        friendRequestRepository.Delete(friendRequestToDelete);
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