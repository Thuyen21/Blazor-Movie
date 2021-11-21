using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

[FirestoreData]
public class FeedbackMessageModel
{
    [FirestoreProperty]
    [Required]
    [EmailAddress]

    public string? Email { get; set; }
    [FirestoreProperty]
    [Required]
    [DataType(DataType.Text)]

    public string? Name { get; set; }
    [Required]
    [FirestoreProperty]
    [DataType(DataType.Text)]

    public string? Message { get; set; }

    [FirestoreProperty]

    public DateTime Time { get; set; }



}
