using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    public class SittingSchedule
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }


        [Required]
        [DisplayName("Sitting Type")]
        public int SittingTypeId { get; set; }

        [Required]
        [DisplayName("Start Time")]
        public DateTime StartTimeId { get; set; }

        [Required]
        [DisplayName("Start Time")]
        public DateTime EndTimeId { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }


        [Required]
        [DisplayName("Sitting Type")]
        public SittingType SittingType { get; set; } = null!;

        [Required]
        [DisplayName("Start Time")]
        public Timeslot StartTime { get; set; } = null!;

        [Required]
        [DisplayName("End Time")]
        public Timeslot EndTime { get; set; } = null!;


    }
}
