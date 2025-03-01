using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Orden> Ordenes { get; set; }
        public virtual DbSet<DetalleOrden> DetallesOrdenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Cliente entity
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
                entity.HasKey(e => e.ClienteId);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Identidad).IsRequired().HasMaxLength(50);
            });

            // Configure Producto entity
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto");
                entity.HasKey(e => e.ProductoId);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            });

            // Configure Orden entity
            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("Orden");
                entity.HasKey(e => e.OrdenId);
                entity.Property(e => e.Impuesto).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime2");

                // Configure relationship with Cliente
                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Ordenes)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Orden_Cliente");
            });

            // Configure DetalleOrden entity
            modelBuilder.Entity<DetalleOrden>(entity =>
            {
                entity.ToTable("DetalleOrden");
                entity.HasKey(e => e.DetalleOrdenId);
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Impuesto).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                // Configure relationship with Orden
                entity.HasOne(d => d.Orden)
                    .WithMany(p => p.DetallesOrdenes)
                    .HasForeignKey(d => d.OrdenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DetalleOrden_Orden");

                // Configure relationship with Producto
                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetallesOrdenes)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DetalleOrden_Producto");
            });
        }
    }
} 