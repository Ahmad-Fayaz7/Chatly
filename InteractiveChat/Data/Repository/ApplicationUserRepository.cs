using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.Models;

namespace InteractiveChat.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ApplicationUser obj)
        {
            _context.ApplicationUsers.Update(obj);
        }
    }
}
