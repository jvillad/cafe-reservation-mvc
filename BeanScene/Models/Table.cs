using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Models
{

    public class Table
    {
        [Key]
        public int Id { get; set; }

        [StringLength(5, MinimumLength = 2)]
        [RegularExpression("^[A-Z]{1,3}\\d{1,3}$", ErrorMessage = "Code pattern must be X1 OR XX001, e.g. A1 B1, AA00, AA01, AB0001")]
        public string Code { get; set; }
        [Required]
        [DisplayName("Area Id")]
        public int AreaId { get; set; }




        // Associations (Navigation Properties)
        public Area? Area { get; set; }
        [DisplayName("Reservations")]
        public List<Reservation> Reservations { get; } = new();


        public Table() { }

        public Table(string code, int areaId)
        {
            Code = code;
            AreaId = areaId;
        }

    }
}
