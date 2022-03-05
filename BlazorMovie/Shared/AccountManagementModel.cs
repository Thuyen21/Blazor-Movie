using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMovie.Shared;

[FirestoreData]
public class AccountManagementModel
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    [Display(Name = "Name")]
    [StringLength(50)]
    public string? Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Role { get; set; }
    public double? Wallet { get; set; }
    public string? UserAgent { get; set; }
    public List<MovieModel> Movies { get; set; } = new();
}

