using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("Parcelas")]
    [Display(Name = "Parcelas")]
    public partial class Parcela : EntityBase
    {
        public Parcela() 
        {
            SembradosDets = new HashSet<SembradoDet>();
        }
        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public decimal Latitud { get; set; } = default!;
        public decimal Longitud { get; set; } = default!;
        public bool Estado { get; set; } = default!;

        [InverseProperty(nameof(SembradoDet.IdParcelaNavigation))]
        public virtual ICollection<SembradoDet> SembradosDets { get; set; } = default!;

    }
}
