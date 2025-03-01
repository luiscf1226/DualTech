using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Shared.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for Producto operations
    /// </summary>
    public class ProductoDto
    {
        /// <summary>
        /// The product ID (should be 0 for creation)
        /// </summary>
        [Required]
        public long ProductoId { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Nombre { get; set; }

        /// <summary>
        /// The product description
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Descripcion { get; set; }

        /// <summary>
        /// The product price
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Precio { get; set; }

        /// <summary>
        /// The product stock quantity
        /// </summary>
        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Existencia must be a non-negative number")]
        public long Existencia { get; set; }
    }
} 