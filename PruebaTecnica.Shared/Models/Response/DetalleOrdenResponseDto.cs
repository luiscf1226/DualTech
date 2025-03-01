namespace PruebaTecnica.Shared.Models.Response
{
    /// <summary>
    /// Response DTO for order detail operations
    /// </summary>
    public class DetalleOrdenResponseDto
    {
        /// <summary>
        /// The order detail ID
        /// </summary>
        public long DetalleOrdenId { get; set; }

        /// <summary>
        /// The order ID associated with this detail
        /// </summary>
        public long OrdenId { get; set; }

        /// <summary>
        /// The product ID associated with this detail
        /// </summary>
        public long ProductoId { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string? NombreProducto { get; set; }

        /// <summary>
        /// The quantity of the product
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// The unit price of the product
        /// </summary>
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// The tax amount for this detail
        /// </summary>
        public decimal Impuesto { get; set; }

        /// <summary>
        /// The subtotal amount for this detail
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// The total amount for this detail
        /// </summary>
        public decimal Total { get; set; }
    }
} 