using System;
using System.Collections.Generic;

namespace PruebaTecnica.Domain.Entities
{
    /// <summary>
    /// Represents a client in the system
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cliente"/> class.
        /// </summary>
        public Cliente()
        {
            Ordenes = new HashSet<Orden>();
        }

        /// <summary>
        /// Gets or sets the client ID (primary key)
        /// </summary>
        /// <example>1</example>
        public long ClienteId { get; set; }

        /// <summary>
        /// Gets or sets the client's name
        /// </summary>
        /// <example>John Doe</example>
        public string Nombre { get; set; }

        /// <summary>
        /// Gets or sets the client's identity document number (must be unique)
        /// </summary>
        /// <example>1234567890</example>
        public string Identidad { get; set; }

        /// <summary>
        /// Gets or sets the collection of orders associated with this client
        /// </summary>
        public virtual ICollection<Orden> Ordenes { get; set; }
    }
} 