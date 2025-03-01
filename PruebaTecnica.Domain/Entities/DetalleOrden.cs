using System;

namespace PruebaTecnica.Domain.Entities
{
    public class DetalleOrden
    {
        public long DetalleOrdenId { get; set; }
        public long OrdenId { get; set; }
        public long ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        // Navigation properties
        public virtual Orden Orden { get; set; }
        public virtual Producto Producto { get; set; }
    }
} 