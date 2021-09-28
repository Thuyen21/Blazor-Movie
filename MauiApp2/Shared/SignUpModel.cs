using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    public class SignUpModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [EmailAddress]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Email { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 7)]
        [PasswordPropertyText]
#pragma warning disable CS8618 // Non-nullable property 'Password' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Password { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Password' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required]
        [DisplayName("ConfirmPassword")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 7)]
        [PasswordPropertyText]
#pragma warning disable CS8618 // Non-nullable property 'ConfirmPassword' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string ConfirmPassword { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ConfirmPassword' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.


        [Required]
        [DisplayName("Name")]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

#pragma warning disable CS8618 // Non-nullable property 'Role' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Role { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Role' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
