using BeanScene.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanScene.Models
{
    public class Reservation
    {
        [Key]
        [DisplayName("Reservation No")]
        public int Id { get; set; }

        [StringLength(450)]
        [DisplayName("User")]
        public string? UserId { get; set; } = null;

        [Required]
        [DisplayName("Sitting Date")]
        public DateTime SittingDate { get; set; }

        [Required]
        [DisplayName("Sitting Sitting Type")]
        public int SittingSittingTypeId { get; set; }

        [Required]
        [DisplayName("Area")]
        public int AreaId { get; set; }

        [Required]
        [DisplayName("Start Time")]
        public DateTime StartTimeId { get; set; }

        [Required]
        [DisplayName("End Time")]
        public DateTime EndTimeId { get; set; }

        [Required]
        [DisplayName("# Guests")]
        [Range(1, 5000)]
        public ushort NumberOfPeople { get; set; } = 1;

        [Required]
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;

        [Required]
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [NotMapped]
        [DisplayName("Full Name")]
        public string FullName { get => $"{FirstName} {LastName}"; }

        [DisplayName("Email")]
        [EmailAddress]
        public string? Email { get; set; } = null!;

        [DisplayName("Phone")]
        public string? Phone { get; set; } = null!;

        [DisplayName("Note")]
        public string? Note { get; set; } = null!;

        [Required]
        [DisplayName("Status")]
        public StatusEnum Status { get; set; }



        public enum StatusEnum
        {
            Pending = 0,
            Confirmed = 1,
            InProgress = 2,
            Completed = 3,
            Cancelled = 4
        }


        // Associations (Navigation Properties)
        [Required]
        [DisplayName("Sitting")]
        [ForeignKey("SittingDate, SittingSittingTypeId")]
        public Sitting Sitting { get; set; } = null!;

        [Required]
        [DisplayName("Area")]
        public Area Area { get; set; } = null!;

        [Required]
        [DisplayName("Start Time")]
        public Timeslot StartTime { get; set; } = null!;

        [Required]
        [DisplayName("End Time")]
        public Timeslot EndTime { get; set; } = null!;

        [DisplayName("Assigned Table")]
        public List<Table> Tables { get; } = new();

        [NotMapped]
        public int Capacity { get; set; } = 4;

        [ForeignKey("UserId")]
        [DisplayName("User")]
        public BeanSceneApplicationUser? User { get; set; } = null;

        [NotMapped]
        public int MinBookingLengthMinutes = 30;
        [NotMapped]
        public int MaxBookingLengthMinutes = 180;

        /// <summary>
        ///     Check if session duration is valid (within the min/max booking lengths)
        /// </summary>
        /// <returns>True if duration is valid</returns>
        public bool IsValidDuration()
        {
            // Check reservation duration (length = end time - start time)
            TimeSpan timeSpan = EndTimeId - StartTimeId;
            return timeSpan.TotalMinutes >= MinBookingLengthMinutes &&
                timeSpan.TotalMinutes <= MaxBookingLengthMinutes;
        }

        /// <summary>
        ///     Check if start time is within the session
        /// </summary>
        /// <returns>True if start time is valid</returns>
        public bool IsValidStartTime()
        {
            return StartTimeId >= Sitting.StartTimeId && StartTimeId <= Sitting.EndTimeId;
        }

        /// <summary>
        ///     Check if end time is within the session
        /// </summary>
        /// <returns>True if end time is valid</returns>
        public bool IsValidEndTime()
        {
            return EndTimeId >= Sitting.StartTimeId && EndTimeId <= Sitting.EndTimeId;
        }
    }
}
