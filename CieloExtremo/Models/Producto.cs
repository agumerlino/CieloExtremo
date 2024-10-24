using System.ComponentModel.DataAnnotations;

namespace CieloExtremo.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? nombre { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? descripcion { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        public string precio { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string categoria { get; set; }
        public string? subcategoria { get; set; }
        // Propiedades para las 6 fotos
        public string? foto1 { get; set; }
        public string? foto2 { get; set; }
        public string? foto3 { get; set; }
        public string? foto4 { get; set; }
        public string? foto5 { get; set; }
        public string? foto6 { get; set; }
        public bool destacar { get; set; }
        [Required(ErrorMessage = "El teléfono del vendedor es obligatorio.")]
        public string? telefonoVendedor { get; set; }
        [Required(ErrorMessage = "El mail del vendedor es obligatorio.")]
        public string? mailVendedor { get; set; }
    }
}
