using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
