using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using Microsoft.EntityFrameworkCore;

namespace InteractiveChat.Data.Repository;

public class FriendRequestRepository(ApplicationDbContext dbContext)
    : Repository<FriendRequest>(dbContext), IFriendRequestRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public FriendRequest? GetBySenderAndReceiverIds(string senderId, string receiverId)
    {
        return _dbContext.FriendRequests
            .FirstOrDefault(request => request.SenderId == senderId && request.ReceiverId == receiverId );
    }

    public IEnumerable<FriendRequestDTO> GetByReceiverId(string id)
    {
        return _dbContext.FriendRequests
            .Where(request => request.ReceiverId == id)
            .Select(request => new FriendRequestDTO()
            {
                SenderUserName = request.SenderUser.UserName,
                SenderFirstName = request.SenderUser.FirstName,
                SenderLastName = request.SenderUser.LastName,
                SenderProfilePicUrl = request.SenderUser.ProfilePicUrl,
                RequestDate = request.InvitationDate
            })
            .ToList();
    } 
}