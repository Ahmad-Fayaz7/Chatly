using Microsoft.AspNetCore.Identity;

namespace InteractiveChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
