using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Shared
{
	public class VipModel
	{
		public string? Id { get; set; }
		[Required]
		public int Choose { get; set; }
	}
}
