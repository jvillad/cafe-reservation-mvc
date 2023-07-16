using BeanScene.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeanScene.ViewModels
{
    public class SittingViewModel
    {
        public Sitting Sitting { get; set; } = null!;
        public IEnumerable<SelectListItem> SittingTypeListItems { get; set; } = new List<SelectListItem>();

        public IEnumerable<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
