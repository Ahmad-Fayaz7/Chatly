using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

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