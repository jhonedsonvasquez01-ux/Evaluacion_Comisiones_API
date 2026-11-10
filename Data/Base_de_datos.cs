using Microsoft.EntityFrameworkCore;
using evaluacion_1.Models;

namespace evaluacion_1.Data
{
    public class Base_de_datos : DbContext
    {
        public Base_de_datos(
            DbContextOptions<Base_de_datos> options
        ) : base(options)
        {
        }

        public DbSet<Vendedor> vendedores { get; set; }
        public DbSet<Venta> ventas { get; set; }
        public DbSet<Comision> comisiones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evitar codigos de vendedor repetidos
            modelBuilder.Entity<Vendedor>()
                .HasIndex(v => v.Codigo)
                .IsUnique();

            // Evitar ventas identicas
            modelBuilder.Entity<Venta>()
                .HasIndex(v => new
                {
                    v.IdVendedor,
                    v.Fecha,
                    v.Monto
                })
                .IsUnique();

            // Una sola comision por venta
            modelBuilder.Entity<Comision>()
                .HasIndex(c => c.IdVenta)
                .IsUnique();

            // Configurar precision de montos
            modelBuilder.Entity<Venta>()
                .Property(v => v.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Comision>()
                .Property(c => c.Porcentaje)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Comision>()
                .Property(c => c.MontoComision)
                .HasPrecision(18, 2);

            // Relacion Vendedor - Ventas
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Vendedor)
                .WithMany()
                .HasForeignKey(v => v.IdVendedor)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacion Venta - Comision
            modelBuilder.Entity<Comision>()
                .HasOne(c => c.Venta)
                .WithOne()
                .HasForeignKey<Comision>(c => c.IdVenta)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}