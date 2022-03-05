using Google.Cloud.Firestore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMovie.Shared;

[FirestoreData]
public class MovieModel
{
    public Guid Id { get; set; }
    public List<AccountManagementModel> Studio { get; set; } = new();

    public string? MovieGenre { get; set; }
    public string? MovieName { get; set; }

    public DateTime? PremiereDate { get; set; }
    public string? MoviesDescription { get; set; }
}

