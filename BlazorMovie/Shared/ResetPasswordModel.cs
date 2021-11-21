using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

public class ResetPasswordModel
{

    [Required]
    [EmailAddress]
    [DisplayName("Email")]

    public string? Email { get; set; }
}

