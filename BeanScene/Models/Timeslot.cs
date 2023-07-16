using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanScene.Models
{
    public class Timeslot
    {
        [Key]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime Time { get; set; }

        [NotMapped]
        public string TimeFormatted { get => Time.ToString("hh:mm tt"); }
    }
}
