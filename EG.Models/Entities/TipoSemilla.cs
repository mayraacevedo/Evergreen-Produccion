using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("TiposSemillas")]
    [Display(Name = "Sembrados")]
    public partial class TipoSemilla : EntityBase
    {
        public TipoSemilla()
        {
            Semillas = new HashSet<Semilla>();
        }

        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public bool Estado { get; set; } = default!;

        [InverseProperty(nameof(Semilla.IdTipoSemillaNavigation))]
        public virtual ICollection<Semilla> Semillas { get; set; } = default!;
    }
}
