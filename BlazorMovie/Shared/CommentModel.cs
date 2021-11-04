using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared
{
    [FirestoreData]
    public class CommentModel
    {
        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("Id")]
        public string? Id { get; set; }
        [FirestoreProperty]

        [DataType(DataType.DateTime)]
        [DisplayName("Time")]
        public DateTime Time { get; set; }

        [FirestoreProperty]

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("MovieId")]
#pragma warning disable CS8618 // Non-nullable property 'MovieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string MovieId { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'MovieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("CommentText")]
#pragma warning disable CS8618 // Non-nullable property 'CommentText' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string CommentText { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'CommentText' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [FirestoreProperty]
        public ulong Like { get; set; }
        [FirestoreProperty]
        public ulong DisLike { get; set; }
        [FirestoreProperty]
        public string? Prediction { get; set; }

        public string? Is { get; set; }
    }
}
