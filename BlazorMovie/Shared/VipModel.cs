using System.ComponentModel.DataAnnotations;

namespace BlazorMovie.Shared;

public class VipModel
{
    [Key]
    public string? Id { get; set; }
    [Required]
    public int Choose { get; set; }
}

