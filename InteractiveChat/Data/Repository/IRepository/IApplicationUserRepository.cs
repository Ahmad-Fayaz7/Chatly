using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
        ApplicationUser GetByUsername(string username);
    }
}
