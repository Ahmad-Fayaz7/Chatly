namespace InteractiveChat.Models;

//[Table("FriendRequest")]
public class FriendRequest
{
    public string SenderId { get; set; }


    public ApplicationUser SenderUser { get; set; }


    public string ReceiverId { get; set; }


    public ApplicationUser ReceiverUser { get; set; }

    public DateTime InvitationDate { get; set; }
}