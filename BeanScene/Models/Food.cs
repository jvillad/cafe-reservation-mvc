using Microsoft.EntityFrameworkCore;

namespace BeanScene.Models
{
	public class Food
	{
		public int FoodId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? ShortDescription { get; set; }
		public string? LongDescription { get; set; }
		public string? AllergyInformation { get; set; }

		[Precision(18, 2)]
		public decimal Price { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageThumbnailUrl { get; set; }
		public bool WeeklySpecial { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; } = default!;
	}
}
