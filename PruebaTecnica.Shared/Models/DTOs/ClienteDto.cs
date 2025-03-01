using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Shared.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Cliente operations
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// The client ID (should be 0 for creation)
        /// </summary>
        [Required]
        public long ClienteId { get; set; }

        /// <summary>
        /// The client's name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Nombre { get; set; }

        /// <summary>
        /// The client's identity document number (must be unique)
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Identity must be between 5 and 50 characters")]
        public string Identidad { get; set; }
    }
} 