using System.ComponentModel.DataAnnotations;

namespace evaluacion_1.DTOs
{
    public class CrearVentaDto
    {
        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Debe indicar un vendedor valido")]
        public int IdVendedor { get; set; }
    }
}