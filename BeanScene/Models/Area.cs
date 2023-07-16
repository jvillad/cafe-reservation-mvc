using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanScene.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Area
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = null!;

    }
}
