using InteractiveChat.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace InteractiveChat.Models
{
    public class FriendRequest
    {

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public FriendRequestStatus Status { get; set; }
    }

}
