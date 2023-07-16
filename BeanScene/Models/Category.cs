using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
	public class Category
	{
		[Required]
		public int CategoryId { get; set; }
		[Required]
		[DisplayName("Category Name")]
		public string CategoryName { get; set; } = string.Empty;
		[DisplayName("Category Description")]
		public string? Description { get; set; }

	}
}
