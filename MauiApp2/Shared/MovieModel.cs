using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    [FirestoreData]
    public class MovieModel
    {
        [FirestoreProperty] public string MovieId { get; set; }
        [FirestoreProperty] public string StudioId { get; set; }

        [FirestoreProperty]
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Movie Genre")]
        public string MovieGenre { get; set; }

        [FirestoreProperty]
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Movie Name")]
        public string MovieName { get; set; }

        [FirestoreProperty]
        [Required]
        [DisplayName("Premiere date")]
        [DataType(DataType.Date)]
        public DateTime PremiereDate { get; set; }
    }
}
