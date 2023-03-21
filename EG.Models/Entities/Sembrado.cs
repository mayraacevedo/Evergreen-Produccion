using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("Sembrados")]
    [Display(Name = "Sembrados")]
    public partial class Sembrado : EntityBase
    {
        [Key]
        public int Id { get; set; } = default!;
        public string Codigo { get; set; } = default!;
       
        [Display(Name = "Estado")]
        public int IdEstado { get; set; } = default!;
        public string? Observaciones { get; set; }
        [NotMapped]
        public string? NombreEstado { get; set; }

        [ForeignKey(nameof(IdEstado))]
        [InverseProperty(nameof(EstadoSembrado.Sembrados))]
        public virtual EstadoSembrado? IdEstadoNavigation { get; set; }

        [InverseProperty(nameof(SembradoDet.IdSembradoNavigation))]
        public virtual ICollection<SembradoDet> SembradosDets { get; set; } = default!;

    }
}
