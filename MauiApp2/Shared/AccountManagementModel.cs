using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    [FirestoreData]
    public class AccountManagementModel
    {
        [FirestoreProperty]
        [Key]
        [DataType(DataType.Text)]
        [DisplayName("Id")]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Email { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [DisplayName("Name")]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]

        public DateTime DateOfBirth { get; set; }

        [FirestoreProperty]
        [Required]
        [DisplayName("Role")]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Role' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Role { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Role' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [DisplayName("Wallet")]
        [DataType(DataType.Text)]
        public double Wallet { get; set; }

        [FirestoreProperty]
        public string? UserAgent { get; set; }
    }
}
