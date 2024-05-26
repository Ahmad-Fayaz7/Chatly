using InteractiveChat.Models;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers;

public class FriendshipController : Controller
{
    private readonly IFriendshipService _friendshipService;
    private readonly UserManager<ApplicationUser> _userManager;
    

    public FriendshipController(UserManager<ApplicationUser> userManager, IFriendshipService friendshipService)
    {
        _userManager = userManager;
        _friendshipService = friendshipService;
        
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string searchTerm)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var searchResults = _friendshipService.SearchFriend(loggedInUser, searchTerm);
        return View(searchResults);
    }

    [HttpPost]
    public async Task<IActionResult> SendFriendRequest(string ReceiverUsername)
    {
        // Get the sender's username
        var senderUsername = _userManager.GetUserName(User);
        // Call the service method to send the friend request
        var result = await _friendshipService.SendFriendRequest(senderUsername, ReceiverUsername);

        if (result.IsSuccess) return Ok(new { success = true, message = "Friend request sent successfully." });

        return BadRequest(new { success = false, message = "Failed to send friend request." });
    }

    [HttpPost]
    public async Task<IActionResult> CancelFriendRequest(string ReceiverUsername)
    {
        Console.WriteLine("The username is: " + ReceiverUsername);
        var loggedInUser = await _userManager.GetUserAsync(User);
        // Call the service method to Cancel the friend request
        var result = _friendshipService.CancelFriendRequest(loggedInUser ,ReceiverUsername);
        return Ok(new { success = true, message = "Friend request canceled successfully." });
    }
}