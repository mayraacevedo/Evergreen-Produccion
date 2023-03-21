using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("EstadosSembrados")]
    [Display(Name = "Estados Sembrados")]
    public partial class EstadoSembrado : EntityBase
    {
        public EstadoSembrado()
        {
            Sembrados = new HashSet<Sembrado>();
        }
        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public bool Estado { get; set; } = default!;


        [InverseProperty(nameof(Sembrado.IdEstadoNavigation))]
        public virtual ICollection<Sembrado> Sembrados { get; set; } = default!;
    }
}
