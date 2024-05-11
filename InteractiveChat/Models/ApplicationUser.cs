using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InteractiveChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Please enter your first name.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime CreationTime { get; set; }

        
    }
}
