using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{
    [PrimaryKey(nameof(Date), nameof(SittingTypeId))]
    public class Sitting
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Sitting Type Id")]
        public int SittingTypeId { get; set; }

        [Required]
        [DisplayName("Start Time")]
        public DateTime StartTimeId { get; set; }

        [Required]
        [DisplayName("End Time")]
        public DateTime EndTimeId { get; set; }


        [DisplayName("Schedule")]
        public int? ScheduleId { get; set; }

        [Required]
        [DisplayName("Status")]
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Available,
            Unavailable
        }

        [Required]
        [DisplayName("Sitting Type")]
        public SittingType SittingType { get; set; } = null!;

        [Required]
        [DisplayName("Start Time")]
        public Timeslot StartTime { get; set; } = null!;

        [Required]
        [DisplayName("End Time")]
        public Timeslot EndTime { get; set; } = null!;


        [DisplayName("Schedule")]
        public SittingSchedule? Schedule { get; set; } = null!;
    }
}
