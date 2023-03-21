using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("SembradosDet")]
    [Display(Name = "Sembrados Detalle")]
    public partial class SembradoDet : EntityBase
    {
        [Key]
        public int Id { get; set; } = default!;
        public int IdSembrado { get; set; } = default!;
        [Display(Name = "Parcela")]
        public int IdParcela { get; set; } = default!;
        [Display(Name = "Semilla")]
        public int IdSemilla { get; set; } = default!;
        [Display(Name = "Estado")]
        public int IdEstado { get; set; } = default!;
        public string? Observaciones { get; set; }
        [NotMapped]
        public string? Index { get; set; }

        [NotMapped]
        public string? NombreParcela { get; set; }
        [NotMapped]
        public string? NombreSemilla { get; set; }
        [NotMapped]
        public string? NombreEstado { get; set; }
        [ForeignKey(nameof(IdSembrado))]
        [InverseProperty(nameof(Sembrado.SembradosDets))]
        public virtual Sembrado? IdSembradoNavigation { get; set; }

        [ForeignKey(nameof(IdParcela))]
        [InverseProperty(nameof(Parcela.SembradosDets))]
        public virtual Parcela? IdParcelaNavigation { get; set; }

        [ForeignKey(nameof(IdSemilla))]
        [InverseProperty(nameof(Semilla.SembradosDets))]
        public virtual Semilla? IdSemillaNavigation { get; set; }

        [ForeignKey(nameof(IdEstado))]
        [InverseProperty(nameof(EstadoSembradoParcela.SembradosDets))]
        public virtual EstadoSembradoParcela? IdEstadoNavigation { get; set; }

    }
}
