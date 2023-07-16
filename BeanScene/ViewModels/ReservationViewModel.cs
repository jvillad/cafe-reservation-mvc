using BeanScene.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeanScene.ViewModels
{
    public class ReservationViewModel
    {
        public Reservation Reservation { get; set; } = null!;
        public string SittingId { get; set; }

        /*public IEnumerable<SelectListItem> TimeslotListItems { get; set; } = new List<SelectListItem>();*/
        public IEnumerable<SelectListItem> SittingsListItems { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AreasListItems { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> UserListItems { get; set; } = new List<SelectListItem>();

        public IEnumerable<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
