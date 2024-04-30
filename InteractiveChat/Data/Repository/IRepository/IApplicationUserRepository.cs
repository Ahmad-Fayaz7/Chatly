using InteractiveChat.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InteractiveChat.Data.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
}
