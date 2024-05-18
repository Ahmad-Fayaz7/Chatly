using InteractiveChat.Models;
using InteractiveChat.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Controllers
{
    public class FriendshipController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendshipController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(string searchTerm)
        {
            List<SearchResultViewModel> searchResultViewModels = new List<SearchResultViewModel>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                var usersWithRelationships = _userManager.Users
                    .Include(u => u.SentFriendRequests) // Include sent friend requests
                    .Include(u => u.ReceivedFriendRequests) // Include received friend requests
                    .Include(u => u.Friends) // Include friendships
                    .Where(u => u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm))
                    .ToList();

                searchResultViewModels = CreateSearchResultViewModel(usersWithRelationships);
            }

            return View(searchResultViewModels);
        }


        private List<SearchResultViewModel> CreateSearchResultViewModel(List<ApplicationUser> usersWithRelationships)
        {
            var loggedInUserId = _userManager.GetUserId(User);
            List<SearchResultViewModel> searchResultViewModels = new List<SearchResultViewModel>();
            // Process the search results
            foreach (var user in usersWithRelationships)
            {
                // Filter and match friend requests and friendships for the searching user and the searched users
                // ToUser means to logged in user
                var sentFriendRequestToUser = user.SentFriendRequests.FirstOrDefault(fr => fr.ReceiverId == loggedInUserId);
                var receivedFriendRequestFromUser = user.ReceivedFriendRequests.FirstOrDefault(fr => fr.SenderId == loggedInUserId);
                var friendshipWithUser = user.Friends.FirstOrDefault(f => f.UserId == loggedInUserId || f.FriendId == loggedInUserId);

                // Use the retrieved data to determine the relationship status between the searching user and the searched user
                if (sentFriendRequestToUser != null) // Logged in user has received a friend request to searched user
                {
                    SearchResultViewModel searchResultViewModel = new SearchResultViewModel
                    {
                        User = user,
                        RelationshipStatus = RelationshipStatus.ReceivedRequest
                    };

                    searchResultViewModels.Add(searchResultViewModel);
                }
                else if (receivedFriendRequestFromUser != null)  // logged in user has sent a friend request to searched user
                {

                    SearchResultViewModel searchResultViewModel = new SearchResultViewModel
                    {
                        User = user,
                        RelationshipStatus = RelationshipStatus.PendingRequest

                    };
                    searchResultViewModels.Add(searchResultViewModel);
                }
                else if (friendshipWithUser != null) // logged in user and searched user are already friends
                {

                    SearchResultViewModel searchResultViewModel = new SearchResultViewModel
                    {
                        User = user,
                        RelationshipStatus = RelationshipStatus.Accepted
                    };
                    searchResultViewModels.Add(searchResultViewModel);
                }
                else
                {
                    SearchResultViewModel searchResultViewModel = new SearchResultViewModel
                    {
                        User = user,
                        RelationshipStatus = RelationshipStatus.None
                    };
                    searchResultViewModels.Add(searchResultViewModel);
                }

            }
            return searchResultViewModels;

        }


    }
}
