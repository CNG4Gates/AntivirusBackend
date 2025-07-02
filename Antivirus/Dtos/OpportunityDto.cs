using System.ComponentModel.DataAnnotations;

namespace Antivirus.DTOs
{
    // DTO para Lectura (GET)
    public class OpportunitiesReadDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } // Elimina nullable si es requerido siempre
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
    }

    // DTO para Creación (POST)
    public class OpportunitiesCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
