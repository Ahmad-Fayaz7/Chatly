using System.ComponentModel.DataAnnotations;

namespace InteractiveChat.DTOs
{
    public class ApplicationUserDTO
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}
