using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

public class ChangeEmailModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [DisplayName("Email")]
    [EmailAddress]

    public string Email { get; set; }

    [Required]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 7)]
    [PasswordPropertyText]
    public string Password { get; set; }
}

