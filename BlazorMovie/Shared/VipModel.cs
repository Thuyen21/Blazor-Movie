using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

public class VipModel
{
    public string? Id { get; set; }
    [Required]
    public int Choose { get; set; }
}

