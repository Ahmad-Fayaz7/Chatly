using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Services.IServices;

public interface IFriendshipService
{
    Task<Result> SendFriendRequest(string senderUsername, string receiverUsername);

    List<SearchResultViewModel> SearchFriend(ApplicationUser? loggedInUser, string searchTerm);
    Result CancelFriendRequest(ApplicationUser loggedInUser, string username);
    Result RejectFriendRequest(ApplicationUser loggedInUser, string username);
    Result AcceptFriendRequest(ApplicationUser loggedInUser, string username);
    IEnumerable<ApplicationUser> GetFriendList(ApplicationUser? loggedInUser);
}