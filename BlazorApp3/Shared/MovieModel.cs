using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
    [FirestoreData]
    public class MovieModel
    {
#pragma warning disable CS8618 // Non-nullable property 'MovieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [FirestoreProperty] public string MovieId { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'MovieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'StudioId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [FirestoreProperty] public string StudioId { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'StudioId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Movie Genre")]
#pragma warning disable CS8618 // Non-nullable property 'MovieGenre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string MovieGenre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'MovieGenre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Movie Name")]
#pragma warning disable CS8618 // Non-nullable property 'MovieName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string MovieName { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'MovieName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [FirestoreProperty]
        [Required]
        [DisplayName("Premiere date")]
        [DataType(DataType.Date)]
        public DateTime PremiereDate { get; set; }
    }
}
