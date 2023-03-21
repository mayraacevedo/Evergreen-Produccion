using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("Semillas")]
    [Display(Name = "Semillas")]
    public partial class Semilla : EntityBase
    {
        public Semilla() 
        {
            SembradosDets = new HashSet<SembradoDet>();
        }

        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public int IdTipoSemilla { get; set; } = default!;
        public bool Estado { get; set; } = default!;

        [ForeignKey(nameof(IdTipoSemilla))]
        [InverseProperty(nameof(TipoSemilla.Semillas))]
        public virtual TipoSemilla? IdTipoSemillaNavigation { get; set; }

        [InverseProperty(nameof(SembradoDet.IdSemillaNavigation))]
        public virtual ICollection<SembradoDet> SembradosDets { get; set; } = default!;
    }
}
