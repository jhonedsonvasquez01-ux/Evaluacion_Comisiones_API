using System.ComponentModel.DataAnnotations;

namespace evaluacion_1.Models
{
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El codigo es requerido")]
        public string Codigo { get; set; } = string.Empty;
    }
}