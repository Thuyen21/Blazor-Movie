using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]

        public string Email { get; set; }
    }
}
