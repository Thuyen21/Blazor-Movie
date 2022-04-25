using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared.Account
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]

        public string Email { get; set; }
    }
}
