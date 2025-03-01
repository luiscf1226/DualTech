namespace PruebaTecnica.Shared.Models.Response
{
    /// <summary>
    /// Response DTO for client operations
    /// </summary>
    public class ClienteResponseDto
    {
        /// <summary>
        /// The client ID
        /// </summary>
        public long ClienteId { get; set; }

        /// <summary>
        /// The client's name
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// The client's identity document number
        /// </summary>
        public string Identidad { get; set; }
    }
} 