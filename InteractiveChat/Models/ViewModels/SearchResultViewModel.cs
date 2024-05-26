namespace InteractiveChat.Models.ViewModels;

public class SearchResultViewModel
{
    public ApplicationUser User { get; set; }
    public RelationshipStatus RelationshipStatus { get; set; }
}

public enum RelationshipStatus
{
    None = 0,
    PendingRequest = 1,
    ReceivedRequest = 2,
    Accepted = 3
}