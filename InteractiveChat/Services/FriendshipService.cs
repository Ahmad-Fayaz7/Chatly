using InteractiveChat.Data;
using InteractiveChat.Data.Repository;
using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.DTOs;
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
        if (string.IsNullOrEmpty(searchTerm) || loggedInUser == null)
        {
            return searchResultViewModels;
        }

        searchTerm = searchTerm.Trim().ToLowerInvariant();
        var searchedUsers = userManager.Users
            .Include(u => u.SentFriendRequests) // Include sent friend requests
            .Include(u => u.ReceivedFriendRequests) // Include received friend requests
            .Include(u => u.Friendships) // Include friendships
            .Include(u => u.FriendsOf) // Include FriendsOf
            .Where(u => (u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm)) &&
                        u.Id != loggedInUser.Id) // Exclude the searching user(Logged in user) 
            .ToList();

        searchResultViewModels = CreateSearchResultViewModel(searchedUsers, loggedInUser.Id);
        return searchResultViewModels;
    }

    public async Task<Result> SendFriendRequest(string senderUsername, string receiverUsername)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(senderUsername) || string.IsNullOrEmpty(receiverUsername))
        {
            return Result.Error("Sender or receiver username is null or empty.");
        }

        // Retrieve sender and receiver users asynchronously
        var sender = applicationUserRepository.GetByUsername(senderUsername);
        var receiver = applicationUserRepository.GetByUsername(receiverUsername);

        // Check if sender or receiver is not found
        if (sender == null || receiver == null)
        {
            return Result.Error("Sender or receiver not found.");
        }

        try
        {
            // Create and add friend request
            var friendRequest = new FriendRequest { SenderId = sender.Id, ReceiverId = receiver.Id, InvitationDate = DateTime.UtcNow };
            friendRequestRepository.Add(friendRequest);

            return Result.Success();
        }
        catch (Exception ex)
        {
            // Log exception or handle error accordingly
            return Result.Error("An error occurred while sending the friend request.");
        }
    }

    public Result CancelFriendRequest(ApplicationUser? loggedInUser, string username)
    {
        // Validate input parameters 
        if (loggedInUser == null || string.IsNullOrEmpty(username))
        {
            return Result.Error("Logged-in user or username is null or empty.");
        }
        // Retrieve the receiver user
        var receiver = applicationUserRepository.GetByUsername(username);
        // Check if the receiver user is not found
        if (receiver == null)
        {
            return Result.Error("Receiver user not found.");
        }

        try
        {
            var friendRequestToDelete = new FriendRequest { SenderId = loggedInUser.Id, ReceiverId = receiver.Id };
            friendRequestRepository.Delete(friendRequestToDelete);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Error("An error occurred while canceling the friend request.");
        }
       
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

            var friendRequestToDelete = friendRequestRepository.GetBySenderAndReceiverIds(sender.Id, loggedInUser.Id);
            if (friendRequestToDelete == null)
            {
                return Result.Error("Friend request does not exist.");
            }

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Friendship friendship = new Friendship() { UserId = sender.Id, FriendId = loggedInUser.Id, FriendshipDate = DateTime.Now};        
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
    public Result RejectFriendRequest(ApplicationUser? loggedInUser, string username)
    {
        // Validate input parameters
        if (loggedInUser == null || string.IsNullOrEmpty(username))
        {
            return Result.Error("Logged-in user or username is null or empty.");
        }
        var sender = applicationUserRepository.GetByUsername(username);
        if (sender == null)
        {
            return Result.Error("Sender not found");
        }

        try
        {
            var friendRequestToDelete = new FriendRequest { SenderId = sender.Id, ReceiverId = loggedInUser.Id };
            friendRequestRepository.Delete(friendRequestToDelete);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Error("An error occurred while canceling the friend request.");
        }
        
    }

    public Result Unfriend(ApplicationUser? loggedInUser, string username)
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
            var friend = applicationUserRepository.GetByUsername(username);
            if (friend == null)
            {
                return Result.Error("The specified user does not exists.");
            }

            var friendshipToDelete = friendshipRepository.GetAll().FirstOrDefault(fr =>
                (fr.UserId == loggedInUser.Id && fr.FriendId == friend.Id) || (fr.FriendId == loggedInUser.Id && fr.UserId == friend.Id));
            if (friendshipToDelete == null)
            {
                return Result.Error("Friendship does not exist.");
            }

            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                friendshipRepository.Delete(friendshipToDelete);
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
        catch (Exception e)
        {
            return Result.Error("An error occurred while processing your request. Please try again later.");
        }
    }
    public IEnumerable<ApplicationUser> GetFriendList(ApplicationUser? loggedInUser)
    {
        IEnumerable<Friendship> friendships = friendshipRepository
            .GetAll()
            .Where(f => f.UserId == loggedInUser?.Id || f.FriendId == loggedInUser?.Id);
        var loggedInUserFriends = friendships.Select(f => f.UserId == loggedInUser?.Id ? f.Friend : f.User);
        return loggedInUserFriends;
    }

    public IEnumerable<FriendRequestDTO> GetReceivedFriendRequests(string id)
    {
        return friendRequestRepository.GetByReceiverId(id);
    }
    private List<SearchResultViewModel> CreateSearchResultViewModel(List<ApplicationUser> searchedUsers,
        string loggedInUserId)
    {
        var searchResultViewModels = new List<SearchResultViewModel>();
        // Process the search results
        foreach (var user in searchedUsers)
        {
            var relationshipStatus = DetermineRelationshipStatus(user, loggedInUserId);
            searchResultViewModels.Add(new SearchResultViewModel { User = user, RelationshipStatus = relationshipStatus });
        }

        return searchResultViewModels;
    }

    private RelationshipStatus DetermineRelationshipStatus(ApplicationUser user, string loggedInUserId)
    {
        var sentFriendRequestToUser = user.SentFriendRequests.FirstOrDefault(fr => fr.ReceiverId == loggedInUserId);
        var receivedFriendRequestFromUser = user.ReceivedFriendRequests.FirstOrDefault(fr => fr.SenderId == loggedInUserId);
        var friendshipWithUser = user.Friendships.FirstOrDefault(f => f.FriendId == loggedInUserId);
        var friendsOfUser = user.FriendsOf.FirstOrDefault(f => f.UserId == loggedInUserId);

        if (sentFriendRequestToUser != null)
            return RelationshipStatus.ReceivedRequest;
        if (receivedFriendRequestFromUser != null)
            return RelationshipStatus.PendingRequest;
        if (friendshipWithUser != null || friendsOfUser != null)
            return RelationshipStatus.Accepted;
        return RelationshipStatus.None;
    }
}