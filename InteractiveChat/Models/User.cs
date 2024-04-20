using System.ComponentModel.DataAnnotations;

namespace InteractiveChat.Models
{
    public class User
    {
        public string Name { get; set; }
        [Key]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string? Status { get; set; }
    }
}
