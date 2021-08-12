using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    [FirestoreData]
    public class CommentModel
    {
        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("Id")]
        public string Id { get; set; }
        [FirestoreProperty]

        [DataType(DataType.DateTime)]
        [DisplayName("Time")]
        public DateTime Time { get; set; }

        [FirestoreProperty]

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }
        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("MovieId")]
        public string MovieId { get; set; }

        [FirestoreProperty]

        [DataType(DataType.Text)]
        [DisplayName("CommentText")]
        public string CommentText { get; set; }

        public int Like { get; set; }

        public int DisLike { get; set; }
        [FirestoreProperty]
        public string Prediction { get; set; }
    }
}
