using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace InteractiveChat.Controllers
{
    public class FriendshipController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(UserManager<ApplicationUser> userManager, IFriendshipService friendshipService)
        {
            _userManager = userManager;
            _friendshipService = friendshipService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string searchTerm)
        {
            var loggedInUserId = _userManager.GetUserId(User);
            List<SearchResultViewModel> searchResults = _friendshipService.SearchFriend(loggedInUserId, searchTerm);
            return View(searchResults);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string username)
        {
            // Get the sender's username (you might need to adjust this based on your authentication mechanism)
            string senderUsername = User.Identity.Name;
            // Call the service method to send the friend request
            var result = await _friendshipService.SendFriendRequest(senderUsername, username);

            if (result.IsSuccess)
            {
                return Ok(new { success = true, message = "Friend request sent successfully." });
            }

            return BadRequest(new { success = false, message = "Failed to send friend request." });
        }

    }
}
