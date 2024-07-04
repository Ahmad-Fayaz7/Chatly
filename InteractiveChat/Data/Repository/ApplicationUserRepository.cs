using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository;

public class ApplicationUserRepository(ApplicationDbContext dbContext)
    : Repository<ApplicationUser>(dbContext), IApplicationUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public ApplicationUser? GetByUsername(string username)
    {
        var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
        return user;
    }

    public void Update(ApplicationUser obj)
    {
        _dbContext.ApplicationUsers.Update(obj);
    }
}