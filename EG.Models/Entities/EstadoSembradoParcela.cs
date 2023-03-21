using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("EstadosSembradosParcelas")]
    [Display(Name = "Estados Sembrados Parcelas")]
    public partial class EstadoSembradoParcela : EntityBase
    {
        public EstadoSembradoParcela()
        {
            SembradosDets = new HashSet<SembradoDet>();
        }
        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public bool Estado { get; set; } = default!;


        [InverseProperty(nameof(SembradoDet.IdEstadoNavigation))]
        public virtual ICollection<SembradoDet> SembradosDets { get; set; } = default!;
    }
}
