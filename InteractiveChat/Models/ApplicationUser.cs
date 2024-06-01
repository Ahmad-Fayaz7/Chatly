using Microsoft.AspNetCore.Identity;

namespace InteractiveChat.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePicUrl { get; set; }

    public DateTime CreationTime { get; set; }

    // Navigation property for sent friend requests
    public ICollection<FriendRequest> SentFriendRequests { get; set; }

    // Navigation property for received friend requests
    public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }

    // Navigation property for friendships
    public ICollection<Friendship> Friendships { get; set; } // Friendships initiated by this user
    public ICollection<Friendship> FriendsOf { get; set; } // Friendships where this user is the friend
}
