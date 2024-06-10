using InteractiveChat.DTOs;
using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Services.IServices;

public interface IFriendshipService
{
    List<SearchResultViewModel> SearchFriend(ApplicationUser? loggedInUser, string searchTerm);
    Task<Result> SendFriendRequest(string senderUsername, string receiverUsername);
    Result CancelFriendRequest(ApplicationUser loggedInUser, string username);
    Result RejectFriendRequest(ApplicationUser loggedInUser, string username);
    Result AcceptFriendRequest(ApplicationUser loggedInUser, string username);
    Result Unfriend(ApplicationUser loggedInUser, string username);
    IEnumerable<ApplicationUser> GetFriendList(ApplicationUser? loggedInUser);
    IEnumerable<FriendRequestDTO> GetReceivedFriendRequests(string loggedInuserId);
}