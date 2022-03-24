using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }

        public string UserAgent { get; set; }
    }
}
