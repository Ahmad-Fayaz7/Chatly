namespace InteractiveChat.Models
{
    public class Friendship
    {

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


        public string FriendId { get; set; }

        public ApplicationUser Friend { get; set; }
        public DateTime FriendshipDate { get; set; }
    }


}
