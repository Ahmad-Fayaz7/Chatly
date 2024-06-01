using AutoMapper;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers;

public class FriendshipController(UserManager<ApplicationUser> userManager, IFriendshipService friendshipService, IMapper mapper)
    : Controller
{
    [HttpGet]
    public  IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string searchTerm)
    {
        var loggedInUser = await userManager.GetUserAsync(User);
        var searchResults = friendshipService.SearchFriend(loggedInUser, searchTerm);
        return View(searchResults);
    }

    [HttpPost]
    public async Task<IActionResult> SendFriendRequest(string username)
    {
        // Get the sender's username
        var senderUsername = userManager.GetUserName(User);
        // Call the service method to send the friend request
        var result = await friendshipService.SendFriendRequest(senderUsername, username);

        if (result.IsSuccess) return Ok(new { success = true, message = "Friend request sent successfully." });

        return BadRequest(new { success = false, message = "Failed to send friend request." });
    }

    [HttpPost]
    public async Task<IActionResult> CancelFriendRequest(string username)
    {
        var loggedInUser = await userManager.GetUserAsync(User);
        // Call the service method to Cancel the friend request
        var result = friendshipService.CancelFriendRequest(loggedInUser ,username);
        return Ok(new { success = true, message = "Friend request canceled successfully." });
    }
    [HttpPost]
    public async Task<IActionResult> RejectFriendRequest(string username)
    {
        var loggedInUser = await userManager.GetUserAsync(User);
        // Call the service method to Cancel the friend request
        var result = friendshipService.RejectFriendRequest(loggedInUser ,username);
        return Ok(new { success = true, message = "Friend request rejected successfully." });
    }

    public async Task<IActionResult> AcceptFriendRequest(string username)
    {
        var loggedInUser = await userManager.GetUserAsync(User);
        if (loggedInUser == null)
        {
            return Unauthorized(new { success = false, message = "User is not authorized." });
        }

        if (string.IsNullOrEmpty(username))
        {
            return BadRequest(new { success = false, message = "Username cannot be null or empty." });
        }

        var result = friendshipService.AcceptFriendRequest(loggedInUser, username);
        if (result.IsSuccess)
        {
            return Ok(new { success = true, message = "Friend request accepted succesfully." });
        }

        return BadRequest(new { success = false, message = result.ErrorMessage });
    }

    [HttpGet]
    public async Task<IActionResult> FriendList()
    {
        var loggedInUser = await userManager.GetUserAsync(User);
        IEnumerable<ApplicationUser> friendList = friendshipService.GetFriendList(loggedInUser);
        if (friendList == null)
        {
            friendList = new List<ApplicationUser>();
        }
        IEnumerable<ApplicationUserDTO> friendListDto = mapper.Map<IEnumerable<ApplicationUserDTO>>(friendList);
        return View(friendListDto);
    }
}