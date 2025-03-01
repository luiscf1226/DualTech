using System;
using System.Collections.Generic;

namespace PruebaTecnica.Shared.Models.Response
{
    /// <summary>
    /// Response DTO for order operations
    /// </summary>
    public class OrdenResponseDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdenResponseDto"/> class.
        /// </summary>
        public OrdenResponseDto()
        {
            Detalles = new List<DetalleOrdenResponseDto>();
        }

        /// <summary>
        /// The order ID
        /// </summary>
        public long OrdenId { get; set; }

        /// <summary>
        /// The client ID associated with this order
        /// </summary>
        public long ClienteId { get; set; }

        /// <summary>
        /// The tax amount for this order
        /// </summary>
        public decimal Impuesto { get; set; }

        /// <summary>
        /// The subtotal amount for this order
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// The total amount for this order
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// The creation date of this order
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// The collection of order details
        /// </summary>
        public List<DetalleOrdenResponseDto> Detalles { get; set; }
    }
} 