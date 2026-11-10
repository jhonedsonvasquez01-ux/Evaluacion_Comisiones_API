using System.ComponentModel.DataAnnotations;

namespace evaluacion_1.DTOs
{
    public class CrearComisionDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Debe indicar una venta valida")]
        public int IdVenta { get; set; }
    }
}