using System.ComponentModel.DataAnnotations;

namespace InteractiveChat.Models
{
    public class Friendship
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FriendId { get; set; }

    }


}
