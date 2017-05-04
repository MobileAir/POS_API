using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class LoginVm
    {
        [Required]
        [Display(Name = "Access Code")]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Pin")]
        public string Password { get; set; }
    }
}