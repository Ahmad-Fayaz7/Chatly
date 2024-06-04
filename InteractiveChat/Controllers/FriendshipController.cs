using AutoMapper;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers
{
    public class FriendshipController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFriendshipService friendshipService;
        private readonly IMapper mapper;

        public FriendshipController(UserManager<ApplicationUser> userManager, IFriendshipService friendshipService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.friendshipService = friendshipService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser == null)
                return UnauthorizedResponse();

            if (string.IsNullOrEmpty(searchTerm))
                return View();

            var searchResults = friendshipService.SearchFriend(loggedInUser, searchTerm);
            return View(searchResults);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string username)
        {
            return await ProcessFriendRequest(username, async (loggedInUser, targetUsername) =>
            {
                var result = await friendshipService.SendFriendRequest(loggedInUser.UserName, targetUsername);
                return result.IsSuccess
                    ? Ok(new { success = true, message = "Friend request sent successfully." })
                    : BadRequest(new
                        { success = false, message = result.ErrorMessage ?? "Failed to send friend request." });
            });
        }

        [HttpPost]
        public async Task<IActionResult> CancelFriendRequest(string username)
        {
            return await ProcessFriendRequest(username, async (loggedInUser, targetUsername) =>
            {
                var result = friendshipService.CancelFriendRequest(loggedInUser, targetUsername);
                return result.IsSuccess
                    ? Ok(new { success = true, message = "Friend request canceled successfully." })
                    : BadRequest(new
                        { success = false, message = result.ErrorMessage ?? "Failed to cancel friend request." });
            });
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(string username)
        {
            return await ProcessFriendRequest(username, async (loggedInUser, targetUsername) =>
            {
                var result = friendshipService.RejectFriendRequest(loggedInUser, targetUsername);
                return result.IsSuccess
                    ? Ok(new { success = true, message = "Friend request rejected successfully." })
                    : BadRequest(new
                        { success = false, message = result.ErrorMessage ?? "Failed to reject friend request." });
            });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(string username)
        {
            return await ProcessFriendRequest(username, async (loggedInUser, targetUsername) =>
            {
                var result = friendshipService.AcceptFriendRequest(loggedInUser, targetUsername);
                return result.IsSuccess
                    ? Ok(new { success = true, message = "Friend request accepted successfully." })
                    : BadRequest(new { success = false, message = result.ErrorMessage });
            });
        }

        [HttpPost]
        public async Task<IActionResult> Unfriend(string username)
        {
            return await ProcessFriendRequest(username, async (loggedInUser, targetUsername) =>
            {
                var result = friendshipService.Unfriend(loggedInUser, targetUsername);
                return result.IsSuccess
                    ? Ok(new { success = true, message = "Unfriended successfully." })
                    : BadRequest(new { success = false, message = result.ErrorMessage });
            });
        }

        [HttpGet]
        public async Task<IActionResult> FriendList()
        {
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser == null)
                return UnauthorizedResponse();

            try
            {
                var friendList = friendshipService.GetFriendList(loggedInUser);
                var friendListDto = mapper.Map<IEnumerable<ApplicationUserDTO>>(friendList);
                return View(friendListDto);
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new { success = false, message = "An error occurred while retrieving the friend list." });
            }
        }

        private async Task<ApplicationUser?> GetLoggedInUserAsync()
        {
            return await userManager.GetUserAsync(User);
        }

        private IActionResult UnauthorizedResponse()
        {
            return Unauthorized(new { success = false, message = "User is not authorized." });
        }

        private async Task<IActionResult> ProcessFriendRequest(string username,
            Func<ApplicationUser, string, Task<IActionResult>> action)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { success = false, message = "Username cannot be empty." });
            }

            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser == null)
            {
                return UnauthorizedResponse();
            }

            try
            {
                return await action(loggedInUser, username);
            }
            catch (Exception)
            {
                return StatusCode(500,
                    new { success = false, message = "An error occurred while processing the friend request." });
            }
        }
    }
}