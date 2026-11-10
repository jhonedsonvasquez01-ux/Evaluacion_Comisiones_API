using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evaluacion_1.Models
{
    public class Comision
    {
        [Key]
        public int Id { get; set; }

        public decimal Porcentaje { get; set; }

        public decimal MontoComision { get; set; }

        public int IdVenta { get; set; }

        [ForeignKey(nameof(IdVenta))]
        public virtual Venta? Venta { get; set; }
    }
}