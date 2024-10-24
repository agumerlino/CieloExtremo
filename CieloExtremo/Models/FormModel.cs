using System.ComponentModel.DataAnnotations;

namespace CieloExtremo.Models
{
    public class FormModel
    {
        [Required]
        [Display(Name = "Nombre y Apellido")]
        public string NombreApellido { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "¿Cómo nos conociste?")]
        public string Conociste { get; set; }

        [Required]
        [Display(Name = "Mensaje, Duda o Consulta")]
        public string Mensaje { get; set; }
    }
}
