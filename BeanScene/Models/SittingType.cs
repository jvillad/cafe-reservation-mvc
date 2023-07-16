using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class SittingType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; } = null!; // Ignore warnings about null values in non-nullable property using the null-forgiving operator (!)
    }
}
