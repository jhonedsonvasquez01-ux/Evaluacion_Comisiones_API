using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluacion_1.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public int IdVendedor { get; set; }

        [ForeignKey(nameof(IdVendedor))]
        public virtual Vendedor? Vendedor { get; set; }
    }
}