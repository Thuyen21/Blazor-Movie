using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3.Shared
{
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
}
