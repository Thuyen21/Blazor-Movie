using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3.Shared
{
   public class VipModel
    {
        public string Id { get; set; }
        [Required]
        public int Choose { get; set; }
    }
}
