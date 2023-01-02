using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Entities.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [ProtectedPersonalData]
    public DateOnly DateOfBirth { get; set; }
    
    [ProtectedPersonalData]
    public double Wallet { get; set; }
}
