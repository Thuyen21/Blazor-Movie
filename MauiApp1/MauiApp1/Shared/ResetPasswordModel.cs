using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    public class ResetPasswordModel
    {

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
