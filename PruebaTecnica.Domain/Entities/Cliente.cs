using System;
using System.Collections.Generic;

namespace PruebaTecnica.Domain.Entities
{
    public class Cliente
    {
        public Cliente()
        {
            Ordenes = new HashSet<Orden>();
        }

        public long ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Identidad { get; set; }

        // Navigation properties
        public virtual ICollection<Orden> Ordenes { get; set; }
    }
} 