namespace PruebaTecnica.Shared.Models.Response
{
    /// <summary>
    /// Response DTO for product operations
    /// </summary>
    public class ProductoResponseDto
    {
        /// <summary>
        /// The product ID
        /// </summary>
        public long ProductoId { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// The product description
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// The product price
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// The product stock quantity
        /// </summary>
        public long Existencia { get; set; }
    }
} 