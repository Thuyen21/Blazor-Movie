using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMovie.Shared;

[FirestoreData]
public class AccountManagementModel
{
    [FirestoreProperty]
    [DataType(DataType.Text)]
    [DisplayName("Id")]
    public Guid Id { get; set; }
    [FirestoreProperty]
    [EmailAddress]
    [DisplayName("Email")]

    public string? Email { get; set; }

    [FirestoreProperty]
    [DisplayName("Name")]
    [DataType(DataType.Text)]
    public string? Name { get; set; }

    [FirestoreProperty]
    [DisplayName("Date of birth")]
    [DataType(DataType.Date)]

    public DateTime DateOfBirth { get; set; }

    [FirestoreProperty]
    [DisplayName("Role")]
    [DataType(DataType.Text)]
    public string? Role { get; set; }

    [FirestoreProperty]
    [DisplayName("Wallet")]
    [DataType(DataType.Text)]
    public double Wallet { get; set; }

    [FirestoreProperty]
    public string? UserAgent { get; set; }
    List<MovieModel> Movies { get; set; }
}

