using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
