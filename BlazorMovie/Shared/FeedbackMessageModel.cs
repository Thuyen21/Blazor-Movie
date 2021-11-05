using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

[FirestoreData]
public class FeedbackMessageModel
{
    [FirestoreProperty]
    [Required]
    [EmailAddress]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Email { get; set; }
    [FirestoreProperty]
    [Required]
    [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Name { get; set; }
    [Required]
    [FirestoreProperty]
    [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Message { get; set; }

    [FirestoreProperty]
#pragma warning disable CS8618 // Non-nullable property 'Email' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public DateTime Time { get; set; }



}
