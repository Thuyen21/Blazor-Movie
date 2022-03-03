using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

public class LogInModel
{
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string? Email { get; set; }

    [Required]
    [DisplayName("Password")]
    [StringLength(50, MinimumLength = 7)]
    [PasswordPropertyText(false)]
    public string? Password { get; set; }

    public string? UserAgent { get; set; }
}
