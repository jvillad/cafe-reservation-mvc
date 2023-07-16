using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Url { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
    }
}
