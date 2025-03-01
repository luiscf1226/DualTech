using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Shared.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new order detail
    /// </summary>
    public class DetalleOrdenCreateDto
    {
        /// <summary>
        /// The product ID associated with this order detail
        /// </summary>
        [Required]
        public long ProductoId { get; set; }

        /// <summary>
        /// The quantity of the product
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Cantidad { get; set; }
    }
} 