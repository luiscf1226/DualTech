using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Shared.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new order
    /// </summary>
    public class OrdenCreateDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdenCreateDto"/> class.
        /// </summary>
        public OrdenCreateDto()
        {
            Detalle = new List<DetalleOrdenCreateDto>();
        }

        /// <summary>
        /// The order ID (should be set to 0 for creation)
        /// </summary>
        [Required]
        public long OrdenId { get; set; }

        /// <summary>
        /// The client ID associated with this order
        /// </summary>
        [Required]
        public long ClienteId { get; set; }

        /// <summary>
        /// The collection of order details
        /// </summary>
        [Required]
        public List<DetalleOrdenCreateDto> Detalle { get; set; }
    }
} 