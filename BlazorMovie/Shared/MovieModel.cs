using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMovie.Shared;

[FirestoreData]
public class MovieModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [FirestoreProperty] public Guid Id { get; set; }
    public AccountManagementModel Studio { get; set; }

    [FirestoreProperty]

    [DataType(DataType.Text)]
    [DisplayName("Movie Genre")]
    public string? MovieGenre { get; set; }

    [FirestoreProperty]

    [DataType(DataType.Text)]
    [DisplayName("Movie Name")]
    public string? MovieName { get; set; }

    [FirestoreProperty]

    [DisplayName("Premiere date")]
    [DataType(DataType.Date)]
    public DateTime PremiereDate { get; set; }


    [FirestoreProperty]

    [DisplayName("Description")]
    [DataType(DataType.Text)]
    public string? MoviesDescription { get; set; }
}

