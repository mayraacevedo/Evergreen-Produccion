using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EG.Models.Entities
{
    [Table("Consecutivos")]
    [Display(Name = "Consecutivos")]
    public partial class ConsecutivoData : EntityBase
    {
       
        [Key]
        public int Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public bool Estado { get; set; } = default!;
        [StringLength(3)]
        public string Prefijo { get; set; } = default!;
        public int Consecutivo { get; set; } = default!;

    }
}
