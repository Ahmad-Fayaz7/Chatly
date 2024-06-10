namespace InteractiveChat.DTOs;

public class FriendRequestDTO
{
    public string? SenderUserName { get; set; }
    public string? SenderFirstName { get; set; }
    public string? SenderLastName { get; set; }
    public string SenderProfilePicUrl { get; set; }
    public DateTime RequestDate { get; set; }
}